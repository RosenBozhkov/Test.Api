using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Common.Resources;
using Config.Models.Health;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Config.Middlewares;

/// <summary>
/// Helper for writing the response of the Health endpoint
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public static class HealthMiddleware
{
    /// <summary>
    /// The method passed for writing the health endpoint
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Task WriteResponse(HttpContext httpContext, HealthReport result)
    {
        var healthModel = new HealthModel
        {
            DeployInformation =
            {
                DeployRelease = EnvironmentHelper.TryGetDeployRelease(),
                DeployId = EnvironmentHelper.TryGetDeployId(),
                InstanceId = EnvironmentHelper.TryGetInstanceId(),
                DeploymentType = EnvironmentHelper.TryGetDeploymentType(),
                ServiceName = EnvironmentHelper.TryGetServiceName(),
                RuntimeSDKVersion = Assembly.GetEntryAssembly()?
                    .GetCustomAttribute<TargetFrameworkAttribute>()?
                    .FrameworkName ?? Constants.NoFrameworkSpecified
            },
            Status = result.Status
        };

        var memoryHealthCheck = result.Entries
            .FirstOrDefault(p => string.Equals(p.Key, "memory", StringComparison.OrdinalIgnoreCase)).Value;

        if (long.TryParse(memoryHealthCheck.Data["AllocatedBytes"].ToString(), out long allocatedBytes))
        {
            healthModel.Results.Memory.Data.AllocatedBytes = allocatedBytes;
        }

        if (long.TryParse(memoryHealthCheck.Data["Gen0Collections"].ToString(), out long gen0Collections))
        {
            healthModel.Results.Memory.Data.Gen0Collections = gen0Collections;
        }

        if (long.TryParse(memoryHealthCheck.Data["Gen1Collections"].ToString(), out long gen1Collections))
        {
            healthModel.Results.Memory.Data.Gen1Collections = gen1Collections;
        }

        if (long.TryParse(memoryHealthCheck.Data["Gen2Collections"].ToString(), out long gen2Collections))
        {
            healthModel.Results.Memory.Data.Gen2Collections = gen2Collections;
        }

        healthModel.Results.Memory.Status = memoryHealthCheck.Status;
        healthModel.Results.Memory.Description = memoryHealthCheck.Description ?? string.Empty;
        healthModel.ThreadsCount = Process.GetCurrentProcess().Threads.Count;

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(healthModel, Formatting.Indented,
            new StringEnumConverter()));
    }
}