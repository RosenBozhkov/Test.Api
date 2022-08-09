using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Common.Exceptions;
using inacs.v8.nuget.EnvHelper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Helpers.Internal;

/// <summary>
/// Helper class for updating configuration from the configuration service. This is done on app startup.
/// If, the config service has no configuration for the service and deployment type, then the current one
/// - combined base and environment - is uploaded. At this point, if an old configuration from the config service
/// is present, in the root of the project, it is ignored.
///
/// If the configuration service has settings, then they are downloaded and take precedence over base and environment settings.
/// Note: They are overriden by secrets and then environment variables.
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class ConfigurationHelper
{
    /// <summary>
    /// Base configuration file name
    /// </summary>
    public const string BaseConfigurationFileName = "appsettings.json";

    /// <summary>
    /// Environment configuration file name
    /// </summary>
    public const string EnvironmentConfigurationFileName = "appsettings.{0}.json";

    /// <summary>
    /// Configuration file name from config service
    /// </summary>
    public const string ConfigurationServiceFileName = "appsettings.json.cache";

    private const string ConfigurationServiceSection = "ConfigurationService";
    private const string FirstVersion = "/1";
    private const string ConfigResponseSection = "result.content";
    private const string SecretsSection = "Secrets";
    private const string SecretsUrlKey = "key_vault_url";
    private const string SecretCredentialsSection = "SecretCredentials";
    private const string SecretsResponseSection = "result";
    private const string ConfigurationSuffix = "configuration";
    private const string EmptyJson = "{}";

    /// <summary>
    /// Updates the existing configuration from the configuration service, if the configuration service has one.
    /// If the config service has none, then uploads the current configuration - combined base and environment.
    /// </summary>
    /// <param name="basePath"></param>
    /// <exception cref="ServiceNotConfiguredException"></exception>
    public static void UpdateConfiguration(string basePath)
    {
        var mainSettings = GetSourceConfiguration(basePath);

        string configurationUrl = GetConfigurationUrl(mainSettings);

        HttpClient client = new()
        {
            BaseAddress = new Uri(configurationUrl)
        };

        string configFilePath = Path.Combine(basePath, ConfigurationServiceFileName);
        if (!File.Exists(configFilePath))
        {
            File.WriteAllText(configFilePath, EmptyJson);
        }
        HttpResponseMessage configurationResponse = client.GetAsync(configurationUrl).Result;

        if (configurationResponse.StatusCode == HttpStatusCode.NotFound)
        {
            // Clear any previous configurations
            File.WriteAllText(configFilePath, EmptyJson);
            UploadLocalConfiguration(mainSettings, client, configurationUrl);
            return;
        }

        string responseBody = configurationResponse.Content.ReadAsStringAsync().Result;
        var responseObject = JObject.Parse(responseBody);
        var content = responseObject.SelectToken(ConfigResponseSection);
        string? configurationContent = content?.ToString();

        if (string.IsNullOrWhiteSpace(configurationContent))
            throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);

        File.WriteAllText(configFilePath, configurationContent);
    }

    private static JObject GetSourceConfiguration(string basePath)
    {
        string baseFilePath = Path.Combine(basePath, BaseConfigurationFileName);
        string evnFileName =
            string.Format(EnvironmentConfigurationFileName, EnvironmentHelper.TryGetDeploymentType());
        string envFilePath = Path.Combine(basePath, evnFileName);

        JObject mainSettings = JObject.Parse(File.ReadAllText(baseFilePath));
        if (File.Exists(envFilePath))
        {
            JObject environmentConfig = JObject.Parse(File.ReadAllText(envFilePath));
            mainSettings.Merge(environmentConfig);
        }

        return mainSettings;
    }

    /// <summary>
    /// Retrieves sensitive configurations for this service from the key vault.
    /// If no vault is specified, or no secret credentials are present, it will
    /// return an empty configuration.
    /// If any of the credentials are invalid, a <exception cref="NotFoundException"></exception> is
    /// thrown.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="basePath"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder, string basePath)
    {
        JObject mainSettings = GetSourceConfiguration(basePath);
        string configFilePath = Path.Combine(basePath, ConfigurationServiceFileName);
        JObject configSettings = JObject.Parse(File.ReadAllText(configFilePath));
        mainSettings.Merge(configSettings);

        JToken? secretCredentials = mainSettings[SecretsSection];
        string? url = Environment.GetEnvironmentVariable(SecretsUrlKey);

        if (secretCredentials is null || url is null)
        {
            return builder;
        }

        HttpClient client = new();

        JObject vaultCredentialsRequest = new()
        {
            [SecretCredentialsSection] = secretCredentials
        };
        var vaultCredentialsAsText = JsonConvert.SerializeObject(vaultCredentialsRequest);
        var data = new StringContent(vaultCredentialsAsText, Encoding.UTF8, Application.Json);
        HttpResponseMessage secretsResponse = client.PostAsync(url, data).Result;

        if (!secretsResponse.IsSuccessStatusCode)
        {
            throw new NotFoundException(Messages.ResourceNotFound);
        }

        JObject response = JObject.Parse(secretsResponse.Content.ReadAsStringAsync().Result);
        JToken? responseBody = response[SecretsResponseSection];
            
        if (responseBody is null) return builder;

        string serializedBody = JsonConvert.SerializeObject(responseBody);
        Stream secretStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedBody));
        builder.AddJsonStream(secretStream);

        return builder;
    }

    private static void UploadLocalConfiguration(JObject mainConfig, HttpClient client,
        string configurationUrl)
    {
        StringContent requestBody =
            new(mainConfig.ToString(Formatting.None), Encoding.UTF8, Application.Json);
        _ = client
            .PostAsync(configurationUrl + FirstVersion, requestBody)
            .Result;
    }

    private static string GetConfigurationUrl(JObject mainConfig)
    {
        string configLocation = mainConfig.Value<string>(ConfigurationServiceSection) ?? string.Empty;

        string deploymentType = EnvironmentHelper.TryGetDeploymentType();

        string serviceName = EnvironmentHelper.TryGetServiceName();

        string configurationUrl = $"{configLocation}/{ConfigurationSuffix}/{serviceName}/{deploymentType}";

        return configurationUrl;
    }
}