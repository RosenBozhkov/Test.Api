using Test.Api.AutoMapper;
using Test.Api.BackgroundServices;
using Business.Implementations.v1;
using Business.Interfaces.v1;
using Common.Configurations;
using Common.Extensions;
using Common.Resources;
using Config.Filters.Action;
using Config.Filters.Auth;
using Config.Filters.ErrorHandling;
using Config.Filters.Result;
using Config.Middlewares;
using Config.Models.Health;
using Config.Swagger.Examples;
using Config.Swagger.Filters;
using Confluent.Kafka;
using FluentValidation.AspNetCore;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using inacs.v8.nuget.ExceptionCollector;
using inacs.v8.nuget.ExposeDeveloper.Implementations;
using inacs.v8.nuget.ExposeDeveloper.Interfaces;
using inacs.v8.nuget.JournalLogs.Implementations;
using inacs.v8.nuget.JournalLogs.Interfaces;
using inacs.v8.nuget.JournalLogs.Persistence;
using inacs.v8.nuget.Kafka;
using inacs.v8.nuget.Kafka.Shared;
using inacs.v8.nuget.MailClient.Configuration;
using inacs.v8.nuget.MailClient.Implementations;
using inacs.v8.nuget.MailClient.Interfaces;
using inacs.v8.nuget.ServiceDiscovery.Contracts;
using inacs.v8.nuget.ServiceDiscovery.Implementation;
using inacs.v8.nuget.ServiceDiscovery.Models;
using inacs.v8.nuget.StoredProcedureBuilder.Implementations;
using inacs.v8.nuget.StoredProcedureBuilder.Interfaces;
using inacs.v8.nuget.Telemetry.Config;
using inacs.v8.nuget.Telemetry.Helpers;
using inacs.v8.nuget.Telemetry.Implementations;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistence.Context.v1;
using Persistence.Implementations.v1;
using Persistence.Interfaces.v1;
using Quartz;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics;
using App.Metrics.Reporting.InfluxDB2;
using Common.Exceptions;
using inacs.v8.nuget.IdentityManager;
using inacs.v8.nuget.IdentityManager.Configurations;
using inacs.v8.nuget.IdentityManager.Implementations;
using inacs.v8.nuget.IdentityManager.Interfaces;
using inacs.v8.nuget.Metrics;
using inacs.v8.nuget.Metrics.Common;
using StackExchange.Redis;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

#pragma warning disable CS1591

namespace Test.Api;

[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class Startup
{
    private const string TelemetryKey = "Telemetry";
    private const string XmlFilePattern = "*.xml";
    private const string ApplicationConfigurationKey = "Application";
    private const string SwaggerConfigurationKey = "Swagger";
    private const string SwaggerEndpointSegment = "swagger.json";
    private const string MetricsConfigurationKey = "Metrics";
    private const string ContextLabel = "App";
    private const string JwtSecurityName = "JWT";
    private const string ApiKeySecurityName = "ApiKey";

    public Startup(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();

        //Controllers
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "dddd, dd MMMM yyyy HH:mm:ss"
            };

            options.SerializerSettings.Converters.Add(dateConverter);
            options.SerializerSettings.Culture = new CultureInfo("en-IE");
            options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            /////otgore sa moi
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });

        //Filters
        services
            .AddMvc(options =>
            {
                //Auth filters
                options.Filters.Add<AuthorizationFilter>();

                //Action filters
                options.Filters.Add<LogRequestFilter>(0);
                options.Filters.Add<ValidateModelStateFilter>(1);
                options.Filters.Add<ObsoleteWarningFilter>(2);
                options.Filters.Add<ActionStatusFilter>(3);

                //Error Handling filters
                options.Filters.Add<ExceptionFilter>();

                //Result filters
                options.Filters.Add<JournalLogFilter>(0);
                options.Filters.Add<LogResponseFilter>(1);
                options.Filters.Add<ErrorInfoFilter>(2);
                options.Filters.Add<MetaFilter>(3);
            })
            .AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<IValidatorService>();
            });

        AddMetrics(services);

        AddSwagger(services);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration.GetSection("JwtParameters:SecretKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

        AddZipkin(services);

        //Health check configuration
        services.AddHealthChecks().AddCheck<MemoryHealthCheck>("Memory");

        AddHostedServices(services);

        AddConfigurations(services);

        // For the enthusiasts doing tests. Sometimes the SD needs to be disabled in tests, so use an if !EnvironmentHelper.IsTest 
        // Configuration for ServiceDiscovery.Client
        services.Configure<ServiceDiscoveryConfig>(Configuration.GetSection("ServiceDiscovery"));
        services.Configure<ServiceDeployInfo>(model =>
        {
            model.ServiceName = EnvironmentHelper.ServiceName!;
            model.Protocol = EnvironmentHelper.Protocol!;
            model.ExternalIp = EnvironmentHelper.ExternalIp!;
            model.Port = EnvironmentHelper.Port!;
            model.ExternalUrl = EnvironmentHelper.ExternalUrl!;
            model.DeployRelease = EnvironmentHelper.DeployRelease!;
            model.DeployId = EnvironmentHelper.DeployId!;
            model.InstanceId = EnvironmentHelper.InstanceId!;
            model.DeploymentType = EnvironmentHelper.DeploymentType!;
        });

        services.AddHttpClient();

        AddApiVersioning(services);

        AddServices(services);

        AddPersistence(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(GlobalErrorHandlingMiddleware.Configure());
        app.UseMiddleware<AttachRequestIdMiddleware>();
        app.UseMiddleware<LogClientIpMiddleware>();
        app.UseMiddleware<CheckDeploymentTypeMiddleware>();
        app.UseMiddleware<TraceParentMiddleware>();
        app.UseMiddleware<NotFoundMiddleware>();
        app.UseMiddleware<ContentLengthRestrictionMiddleware>();
        app.UseMiddleware<GracefulMiddleware>();
        app.UseMiddleware<EnableRequestBufferingMiddleware>();

        app.UseLocalization();

        app.UseCors(options =>
            options.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

        UseSwagger(app);

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            var applicationConfig = new ApplicationConfiguration();
            Configuration.Bind(ApplicationConfigurationKey, applicationConfig);
            endpoints.MapControllers();
            endpoints.MapHealthChecks(applicationConfig.HealthEndpoint, new HealthCheckOptions()
            {
                ResponseWriter = HealthMiddleware.WriteResponse
            });
        });
    }

    private void UseSwagger(IApplicationBuilder app)
    {
        if (EnvironmentHelper.IsProduction) return;

        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add(ProxyPrefixFilter.ReplaceProxyPrefix);
            c.RouteTemplate = "swagger/{documentName}/" + SwaggerEndpointSegment;
        });
        app.UseSwaggerUI(c =>
        {
            string serviceName = EnvironmentHelper.TryGetServiceName();
            var swaggerConfig = new SwaggerConfiguration();
            Configuration.Bind(SwaggerConfigurationKey, swaggerConfig);
            var firstDocument = swaggerConfig.Documents[0];

            c.DocumentTitle = $"{serviceName} API - Swagger UI";
            c.SwaggerEndpoint($"{firstDocument.Name}/{SwaggerEndpointSegment}",
                $"{serviceName} API {firstDocument.Name}");
            c.InjectStylesheet("css/style.css");
            c.InjectJavascript("css/script.js");
        });
    }

    private void AddPersistence(IServiceCollection services)
    {
        //Repositories needed
        services.AddScoped<IPrincipalClient, PrincipalClient>();
        services.AddScoped<IDatabase>(provider =>
        {
            ConnectionMultiplexer multiplexer =
                provider.GetService<ConnectionMultiplexer>()
                ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
            return multiplexer.GetDatabase();
        });

        //Repositories custom
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();
        services.AddScoped<IMakeRepository, MakeRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Db contexts needed
        services.AddDbContext<ExceptionsContext>(
            optionsBuilder => optionsBuilder.UseMySQL(Configuration.GetConnectionString("ExceptionCollector")));
        services.AddDbContext<JournalContext>(
            optionsBuilder => optionsBuilder.UseMySQL(Configuration.GetConnectionString("JournalCollector")));
        //Db contexts custom 
        services.AddDbContext<ThingsContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("TestApiDatabase")));

#warning remove kafka producer registration, if not needed(i will most certainly use the kafka)
        services.AddStandardAndTransactionalKafkaProducers(
            standardProducerBuilder:
            new ProducerBuilder<string, string>(new ProducerConfig
            {
                BootstrapServers = Configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Brokers").Value,
                Acks = Acks.All,
                EnableIdempotence = true,
                //Debug = Configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Debug").Value,
                ClientId = $"stdproducer:{EnvironmentHelper.TryGetExternalIp()}:{EnvironmentHelper.TryGetPort()}",
            }),
            transactionalProducerBuilder:
            new ProducerBuilder<string, string>(new ProducerConfig
            {
                BootstrapServers = Configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Brokers").Value,
                Acks = Acks.All,
                EnableIdempotence = true,
                TransactionalId = $"{EnvironmentHelper.TryGetExternalIp()}:{EnvironmentHelper.TryGetPort()}",
                //Debug = Configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Debug").Value,
                ClientId = $"txnproducer:{EnvironmentHelper.TryGetExternalIp()}:{EnvironmentHelper.TryGetPort()}"
            })
        );
    }

    private void AddServices(IServiceCollection services)
    {
        services.AddSingleton<RequestsTracker>();
        services.AddSingleton<IEmailSender, EmailSender>();
        services.AddSingleton<IExposeDeveloperService, ExposeDeveloperService>();
        services.ConfigureAutomapper();
        services.AddSingleton(sp =>
        {
            IMetrics metrics = sp.GetService<IMetrics>()
                               ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
            return metrics.Measure;
        });
        services.AddScoped<IValidatorService, ValidatorService>();
        services.AddScoped(sp =>
        {
            var telemetryProvider = sp.GetService<ITelemetryProvider>()
                                    ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
            Guid requestId = telemetryProvider.GetTraceId();
            return new RequestState(requestId);
        });
        services.AddSingleton(_ =>
        {
            string connectionString = Configuration.GetConnectionString("RedisCache")
                                      ?? throw new ServiceNotConfiguredException(Messages.GeneralErrorMessage);
            return ConnectionMultiplexer.Connect(connectionString);
        });
        services.AddScoped<IStoredProcedureBuilder, StoredProcedureBuilder>();
        services.AddScoped<ITelemetryProvider, TelemetryProvider>();
        services.AddScoped<IServiceUriProvider, ServiceUriProvider>();
        services.AddScoped<IExceptionCollector, ExceptionCollector>();
        services.AddScoped<IIdentityValidatorService, IdentityValidatorService>();
        services.AddScoped<IJournalCollector, JournalCollector>();

        //Services custom
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IModelService, ModelService>();
        services.AddScoped<IMakeService, MakeService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IUserService, UserService>();
    }

    private void AddConfigurations(IServiceCollection services)
    {
        services.Configure<JwtConfiguration>(Configuration.GetSection("JwtParameters"));
        services.Configure<TelemetryConfiguration>(Configuration.GetSection(TelemetryKey));
        services.Configure<SmtpConfiguration>(Configuration.GetSection("SmtpConfiguration"));
        services.Configure<ApplicationConfiguration>(Configuration.GetSection(ApplicationConfigurationKey));
        //Runtime configurations
        services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
        services.Configure<HostOptions>(o => o.ShutdownTimeout = TimeSpan.FromMinutes(10));
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }

    private void AddMetrics(IServiceCollection services)//tuka e edno mqsto kudeto moje da bude
    {
        var config = new MetricsConfiguration();
        Configuration.Bind(MetricsConfigurationKey, config);
        IMetricsBuilder metrics = new MetricsBuilder()
            .SampleWith.ForwardDecaying()
            .Configuration.Configure(c =>
            {
                c.DefaultContextLabel = ContextLabel;
                c.GlobalTags.Add(EnvironmentHelper.DeploymentTypeKey, EnvironmentHelper.TryGetDeploymentType());
                c.GlobalTags.Add(EnvironmentHelper.ServiceNameKey, EnvironmentHelper.TryGetServiceName());
                c.GlobalTags.Add(EnvironmentHelper.InstanceIdKey, EnvironmentHelper.TryGetInstanceId());
            })
            .MetricFields.Configure(fields =>
            {
                fields.Histogram.OnlyInclude(HistogramFields.Mean, HistogramFields.P95, HistogramFields.P99);
            })
            .Report.ToInfluxDb2(new MetricsReportingInfluxDb2Options
            {
                InfluxDb2 = new InfluxDb2Options
                {
                    Bucket = config.Bucket,
                    Organization = config.Organization,
                    Token = config.Token,
                    BaseUri = new Uri(config.Url)
                },
                FlushInterval = TimeSpan.FromSeconds(config.FlushInterval)
            });

        services.AddMetrics(metrics);

        if (EnvironmentHelper.IsLocal)
        {
            services.AddMetricsEndpoints();
        }

        services.AddPerformanceMetrics();

        services.AddMetricsReportingHostedService();
    }

    private void AddZipkin(IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(builder =>
        {
            var config = new TelemetryConfiguration();
            Configuration.Bind(TelemetryKey, config);
            builder
                .SetSampler(new TraceIdRatioBasedSampler(config.Ratio))
                .AddAspNetCoreInstrumentation((options) =>
                {
                    options.Enrich = (activity, _, _) => activity.AddEnvironmentVariables();
                    options.Filter = _ => config.IsActive;
                })
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(EnvironmentHelper.TryGetServiceName()))
                .AddSource(config.SourcePattern)
                .AddZipkinExporter(zipkinOptions => zipkinOptions.Endpoint = new Uri(config.URL));
        });
    }

    /// <summary>
    /// Hosted services that are not from another library
    /// </summary>
    /// <param name="services"></param>
    private static void AddHostedServices(IServiceCollection services)
    {
        if (!EnvironmentHelper.IsLocal)
        {
            services.AddHostedService<LifetimeEventsHostedService>();
        }

#warning remove ExampleKafkaConsumer if not needed otherwise modify
        services.AddHostedService<ExampleKafkaConsumer>();
#warning remove if not needed
        services.AddHostedService<ExampleBackgroundService>();
    }

    private void AddApiVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            var applicationConfig = new ApplicationConfiguration();
            Configuration.Bind(ApplicationConfigurationKey, applicationConfig);
            options.AssumeDefaultVersionWhenUnspecified = applicationConfig.AssumeDefaultVersion;
            options.DefaultApiVersion =
                new ApiVersion(applicationConfig.DefaultMajorApiVersion, applicationConfig.DefaultMinorApiVersion);
            options.ReportApiVersions = true;
            options.UseApiBehavior = false;
            options.ApiVersionReader = new HeaderApiVersionReader(applicationConfig.VersionHeaderKey);
            options.ErrorResponses = new ApiVersionErrorResponse(applicationConfig);
        });
    }

    private void AddSwagger(IServiceCollection services)
    {
        if (EnvironmentHelper.IsProduction) return;

        services.AddSwaggerGen(options =>
        {
            string serviceName = EnvironmentHelper.TryGetServiceName();

            var swaggerConfig = new SwaggerConfiguration();
            Configuration.Bind(SwaggerConfigurationKey, swaggerConfig);
            var firstDocument = swaggerConfig.Documents[0];
            options.SwaggerDoc(firstDocument.Name,
                new OpenApiInfo
                {
                    Title = $"{serviceName} API",
                    Version = firstDocument.Version
                });

            options.AddSecurityDefinition(JwtSecurityName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = Messages.EnterJwt,
                Name = IdentityConstants.AuthorizationHeader,
                Type = SecuritySchemeType.Http,
                BearerFormat = JwtSecurityName,
                Scheme = IdentityConstants.BearerTokenType
            });

            options.AddSecurityDefinition(ApiKeySecurityName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = Messages.EnterApiKey,
                Name = IdentityConstants.AuthorizationHeader,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = ApiKeySecurityName,
                Scheme = IdentityConstants.ApiKeyType
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtSecurityName
                        }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApiKeySecurityName
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.ExampleFilters();

            options.DocumentFilter<HideActionFilter>();
            options.DocumentFilter<ExcludeSchemaFilter>();

            options.SchemaFilter<EnumSchemaFilter>();
            options.SchemaFilter<ExcludeFieldFromSchemaFilter>();

            var applicationConfig = new ApplicationConfiguration();
            Configuration.Bind(ApplicationConfigurationKey, applicationConfig);
            options.OperationFilter<AddVersionHeader>(applicationConfig);
            options.OperationFilter<ExcludeOperationParameterFilter>();

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            List<string> xmlFiles = Directory
                .GetFiles(AppContext.BaseDirectory, XmlFilePattern, SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
        });

        services.AddFluentValidationRulesToSwagger();
        services.AddSwaggerExamplesFromAssemblyOf(typeof(ExposedDevelopersInformationExample));
        services.AddSwaggerGenNewtonsoftSupport();
    }
}