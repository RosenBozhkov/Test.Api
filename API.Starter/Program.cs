global using inacs.v8.nuget.Core.Models;
global using inacs.v8.nuget.IdentityManager.Attributes;
global using inacs.v8.nuget.Telemetry.Interfaces;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using Common.Configurations;
using Common.Exceptions;
using Common.Helpers.Internal;
using Common.Resources;
using Config.Filters.Auth;
using Config.Middlewares;
using Config.SerilogUtils;
using inacs.v8.nuget.EnvHelper;
using inacs.v8.nuget.StoredProcedureBuilder.Models;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

[assembly: Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[assembly: Developer("Vasil Egov", "v.egov@itsoft.bg")]

#pragma warning disable CS1591

namespace Test.Api;

[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class Program
{
    private const string SourceContext = "SourceContext";
    private const string LoggingSectionKey = "LoggingConfig";

    private static readonly string StoredProcedureContext =
        typeof(CommandInstance).FullName
        ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
    
    private static readonly string AuthorizationFilterContext =
        typeof(AuthorizationFilter).FullName
        ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);

    private static readonly string ClientIpContext =
        typeof(LogClientIpMiddleware).FullName
        ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder host = Host.CreateDefaultBuilder(args)
            .ConfigureAppSettings()
            .ConfigureLogging()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(DeployHelper.GetDeployAddress())
                    .UseEnvironment(EnvironmentHelper.TryGetDeploymentType())
                    .UseStartup<Startup>();
            });

        return host;
    }
    private static IHostBuilder ConfigureAppSettings(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration(config =>
        {
            string basePath =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                ?? string.Empty;
            string env = EnvironmentHelper.TryGetDeploymentType();
            string environmentConfigurationFile =
                string.Format(ConfigurationHelper.EnvironmentConfigurationFileName, env);
            ConfigurationHelper.UpdateConfiguration(basePath);

            // The logging must be here, as if it is before this, the configuration settings
            // from the config service will still be old ones
            EnvironmentVariablesLogHelper.LogEnvironmentVariables(basePath);
            config
                .SetBasePath(basePath)
                .AddJsonFile(path: ConfigurationHelper.BaseConfigurationFileName, optional: false)
                .AddJsonFile(path: environmentConfigurationFile, optional: EnvironmentHelper.IsLocal)
                .AddJsonFile(path: ConfigurationHelper.ConfigurationServiceFileName, optional: false)
                .AddSecrets(basePath)
                .AddEnvironmentVariables();
        });

        return hostBuilder;
    }

    private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch();
        hostBuilder
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseSerilog((hostBuilderContext, loggerConfiguration) =>
            {
                bool IsStoredProcedure(LogEvent logEvent)
                {
                    var value = (logEvent.Properties[SourceContext] as ScalarValue)?.Value as string;
                    return value == StoredProcedureContext;
                }

                bool IsClientIpLog(LogEvent logEvent)
                {
                    var value = (logEvent.Properties[SourceContext] as ScalarValue)?.Value as string;
                    return value == ClientIpContext || value == AuthorizationFilterContext;
                }

                bool IsGeneral(LogEvent logEvent)
                {
                    var value = (logEvent.Properties[SourceContext] as ScalarValue)?.Value as string;
                    return value != ClientIpContext && value != StoredProcedureContext;
                }
                    
                bool IsLocalOrTest(LogEvent logEvent)
                {
                    var value = logEvent.Properties[EnvironmentHelper.DeploymentTypeKey] as ScalarValue;
                    return value?.Value as string == EnvironmentHelper.LocalDeploymentType
                           || value?.Value as string == EnvironmentHelper.TestDeploymentType;
                }

                var config = new LoggingConfiguration();
                hostBuilderContext.Configuration.Bind(LoggingSectionKey, config);

                loggerConfiguration
                    .MinimumLevel.ControlledBy(loggingLevelSwitch);
                foreach (var source in config.Override)
                {
                    loggerConfiguration.MinimumLevel.Override(source, LogEventLevel.Warning);
                }

                loggerConfiguration
                    .Enrich.With<DefaultLevelEnricher>()
                    .Enrich.WithProperty(EnvironmentHelper.ServiceNameKey, EnvironmentHelper.TryGetServiceName())
                    .Enrich.WithProperty(EnvironmentHelper.DeploymentTypeKey,
                        EnvironmentHelper.TryGetDeploymentType());
                foreach (var (key, value) in config.Properties)
                {
                    loggerConfiguration.Enrich.WithProperty(key, value);
                }

                //loggerConfiguration.WriteTo.Logger(lokiConfiguration =>
                //{
                //    lokiConfiguration.WriteTo.GrafanaLoki(
                //            uri: config.LokiUrl,
                //            outputTemplate: config.LokiTemplate,
                //            filtrationLabels: config.LokiLabels,
                //            filtrationMode: LokiLabelFiltrationMode.Include,
                //            textFormatter: new LokiJsonTextFormatter()
                //        )
                //        .Filter.ByIncludingOnly(IsGeneral);
                //});

                loggerConfiguration.WriteTo.Logger(fileConfiguration =>
                {
                    fileConfiguration.WriteTo.Async(
                            asyncWrapper =>
                            {
                                string path = Path.Combine(EnvironmentHelper.TryGetLogsDir(),
                                    string.Format(config.FileName, EnvironmentHelper.TryGetServiceName(),
                                        EnvironmentHelper.TryGetInstanceId()));
                                asyncWrapper.File(
                                    path: path,
                                    outputTemplate: config.FileTemplate,
                                    buffered: config.IsBuffered,
                                    rollingInterval: Enum.Parse<RollingInterval>(config.RollingInterval),
                                    retainedFileCountLimit: config.RetainedFileCount,
                                    flushToDiskInterval: TimeSpan.FromSeconds(config.FlushInterval)
                                );
                            },
                            blockWhenFull: config.WilLBlockWhenFull)
                        .Filter.ByIncludingOnly(IsGeneral);
                });

                loggerConfiguration.WriteTo.Logger(clientIpLog =>
                {
                    clientIpLog.WriteTo.Async(asyncWrapper =>
                        {
                            string path = Path.Combine(EnvironmentHelper.TryGetLogsDir(),
                                string.Format(config.IpName,
                                    EnvironmentHelper.TryGetServiceName(),
                                    EnvironmentHelper.TryGetInstanceId()));
                            asyncWrapper.File(
                                path: path,
                                outputTemplate: config.IpTemplate,
                                buffered: config.IsBuffered,
                                rollingInterval: Enum.Parse<RollingInterval>(config.RollingInterval),
                                retainedFileCountLimit: config.RetainedFileCount,
                                flushToDiskInterval: TimeSpan.FromSeconds(config.FlushInterval)
                            );
                        }, blockWhenFull: config.WilLBlockWhenFull)
                        .Filter.ByIncludingOnly(IsClientIpLog);
                });

                loggerConfiguration
                    .WriteTo.Logger(consoleConfiguration =>
                    {
                        consoleConfiguration
                            .WriteTo.Console(outputTemplate: config.ConsoleTemplate)
                            .Filter.ByIncludingOnly(IsLocalOrTest);
                    });
            })
            .ConfigureServices(collection => { collection.AddSingleton(loggingLevelSwitch); });

        return hostBuilder;
    }
}