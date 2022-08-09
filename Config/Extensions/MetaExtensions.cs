using System.IO;
using Common.Configurations;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Http;

namespace Config.Extensions;

[Developer("Vasil Egov", "v.egov@itsoft.bg")]
internal static class MetaExtensions
{
    internal static void Initialize(this Meta meta, RequestState requestState, HttpContext context, int instanceId,
        ApplicationConfiguration applicationConfiguration)
    {
        string proxyPrefix = context.Request.GetReverseProxyPrefix();
        meta.RequestId = requestState.RequestId.ToString();
        meta.ExecutionTimeMs = requestState.ExecutionTimeMs;
        meta.ExecutionFinishedUTC = requestState.ExecutionFinishedUTC;
        meta.HttpMethod = context.Request.Method;
        meta.Path = Path.Join(proxyPrefix, context.Request.Path);
        bool isSelected = !context.Request.Headers[applicationConfiguration.VersionHeaderKey].IsEmpty();
        meta.Version = isSelected
            ? context.Request.Headers[applicationConfiguration.VersionHeaderKey].ToString()
            : applicationConfiguration.DefaultApiVersion;
        meta.InstanceId = instanceId;
        meta.HealthEndpoint = Path.Join(proxyPrefix, applicationConfiguration.HealthEndpoint);
    }
}