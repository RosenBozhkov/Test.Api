using Common.Configurations;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Config.Extensions;

[Developer("Vasil Egov", "v.egov@itsoft.bg")]
internal static class ErrorResponseHelper
{
    internal static string Convert(ResponseContent response)
    {
        return JsonConvert.SerializeObject(response, new StringEnumConverter());
    }

    internal static ResponseContent GenerateErrorResponseContent(
        HttpContext context,
        RequestState requestState,
        Error error,
        int instanceId,
        ApplicationConfiguration applicationConfiguration)
    {
        Meta meta = new();
        meta.Initialize(requestState, context, instanceId, applicationConfiguration);

        ResponseContent response = new()
        {
            Error = error,
            Meta = meta
        };
        return response;
    }
}