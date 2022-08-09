using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Newtonsoft.Json.Linq;

namespace Common.Helpers.Internal;

/// <summary>
/// Helper for logging initial configuration of service before start.
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class EnvironmentVariablesLogHelper
{
    private static readonly string Separator = new('=', 80);
    private const int Offset = -35;

    /// <summary>
    /// Logs all the environment variables and configuration before the start of the program
    /// </summary>
    public static void LogEnvironmentVariables(string basePath)
    {
        StringBuilder sb = new();
        AppendEnvironmentVariables(sb);

        string baseFilePath = Path.Combine(basePath, ConfigurationHelper.BaseConfigurationFileName);
        string evnFileName =
            string.Format(ConfigurationHelper.EnvironmentConfigurationFileName,
                EnvironmentHelper.TryGetDeploymentType());
        string envFilePath = Path.Combine(basePath, evnFileName);
        string configFilePath = Path.Combine(basePath, ConfigurationHelper.ConfigurationServiceFileName);

        JObject mainConfig = JObject.Parse(File.ReadAllText(baseFilePath));
        sb.Append("Main configuration:").AppendLine();
        sb.Append(mainConfig);
        sb.AppendLine().Append(Separator).AppendLine();

        JObject? environmentConfig = null;
        if (File.Exists(envFilePath))
        {
            environmentConfig = JObject.Parse(File.ReadAllText(envFilePath));
            sb.Append("Environment configuration:").AppendLine();
            sb.Append(environmentConfig);
            sb.AppendLine().Append(Separator).AppendLine();
        }

        JObject configurationServiceConfig = JObject.Parse(File.ReadAllText(configFilePath));
        sb.Append("Configure service configuration:").AppendLine();
        sb.Append(configurationServiceConfig);
        sb.AppendLine().Append(Separator).AppendLine();

        AppendOverrides(mainConfig, environmentConfig, configurationServiceConfig, sb);

        if (EnvironmentHelper.IsLocal)
        {
            Console.WriteLine(sb.ToString());
        }
            
        if (!Directory.Exists(EnvironmentHelper.TryGetLogsDir()))
        {
            Directory.CreateDirectory(EnvironmentHelper.TryGetLogsDir());
        }
            
        string logPath = Path.Combine(EnvironmentHelper.TryGetLogsDir(),
            $"startup.{EnvironmentHelper.TryGetServiceName()}.{EnvironmentHelper.TryGetInstanceId()}.log");
        File.WriteAllText(logPath, sb.ToString());
    }

    private static void AppendEnvironmentVariables(StringBuilder sb)
    {
        List<string> environmentVariableKeys = new();
        foreach (string key in Environment.GetEnvironmentVariables().Keys)
        {
            environmentVariableKeys.Add(key);
        }

        environmentVariableKeys = environmentVariableKeys.OrderBy(k => k).ToList();

        sb.Append("Environment variables on container startup").AppendLine();
        foreach (string key in environmentVariableKeys)
        {
            sb.Append($"{key,Offset} -> {Environment.GetEnvironmentVariables()[key]}").AppendLine();
        }

        sb.Append(Separator).AppendLine();
    }

    private static void AppendOverrides(
        JObject mainConfig,
        JObject? environmentConfig,
        JObject configurationServiceConfig,
        StringBuilder sb)
    {
        ISet<string> baseAndEnvOverrides = new HashSet<string>();
        ISet<string> envAndConfigOverrides = new HashSet<string>();
        if (environmentConfig is not null)
        {
            baseAndEnvOverrides = GetOverrides(mainConfig, environmentConfig);
            envAndConfigOverrides = GetOverrides(environmentConfig, configurationServiceConfig);
        }

        ISet<string> baseAndConfigOverrides = GetOverrides(mainConfig, configurationServiceConfig);

        sb.Append("Base And Environment overrides:").AppendLine();
        foreach (var key in baseAndEnvOverrides)
        {
            sb.Append(key).AppendLine();
        }

        sb.Append(Separator).AppendLine();

        sb.Append("Base And Configuration service overrides:").AppendLine();
        foreach (var key in baseAndConfigOverrides)
        {
            sb.Append(key).AppendLine();
        }

        sb.Append(Separator).AppendLine();

        sb.Append("Environment And Configuration service overrides:").AppendLine();
        foreach (var key in envAndConfigOverrides)
        {
            sb.Append(key).AppendLine();
        }

        sb.Append(Separator).AppendLine();
    }

    private static ISet<string> GetOverrides(JObject first, JObject second)
    {
        ISet<string> firstObjectKeys = new HashSet<string>();
        ISet<string> secondObjectKeys = new HashSet<string>();
        GetConfigurationKeys(first, firstObjectKeys);
        GetConfigurationKeys(second, secondObjectKeys);

        firstObjectKeys.IntersectWith(secondObjectKeys);

        return firstObjectKeys;
    }

    private static void GetConfigurationKeys(JToken obj, ISet<string> keys)
    {
        foreach (var token in obj.Children().ToList())
        {
            if (token is JProperty)
            {
                keys.Add(token.Path);
            }

            GetConfigurationKeys(token, keys);
        }
    }
}