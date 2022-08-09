using System.IO;
using API.Starter.Tests.Extensions;
using Common.Helpers.Internal;
using inacs.v8.nuget.EnvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Api;

namespace API.Starter.Tests.Fixtures;

/// <summary>
/// A fixture for creating a DI container with our apps settings. This is needed, if for example
/// you want to mock a service called directly from the controller, without mocking the whole application.
/// This will give you the ability to resolve most of the actual dependencies and mock just a few of them by replacing
/// them in the collection
/// </summary>
public class DependencyContainerFixture
{
    internal ServiceCollection ServiceCollection { get; } = new();

    public DependencyContainerFixture()
    {
        var config = new ConfigurationBuilder();
        string basePath =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            ?? string.Empty;
        string env = EnvironmentHelper.TryGetDeploymentType();
        string environmentConfigurationFile =
            string.Format(ConfigurationHelper.EnvironmentConfigurationFileName, env);
        ConfigurationHelper.UpdateConfiguration(basePath);
        config
            .SetBasePath(basePath)
            .AddJsonFile(path: ConfigurationHelper.BaseConfigurationFileName, optional: false,
                reloadOnChange: true)
            .AddJsonFile(path: environmentConfigurationFile, optional: EnvironmentHelper.IsLocal,
                reloadOnChange: true)
            .AddJsonFile(path: ConfigurationHelper.ConfigurationServiceFileName, optional: false,
                reloadOnChange: true)
            .AddEnvironmentVariables();

        new Startup(config.Build()).ConfigureServices(ServiceCollection);

        ServiceCollection.RemoveHostedServices();
    }
}