using Config.Extensions;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace Config.Swagger.Filters;

/// <summary>
/// Swagger filter that adds proxy prefixes to paths
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public static class ProxyPrefixFilter
{
    /// <summary>
    /// Adds proxy prefix to route if needed
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="request"></param>
    public static void ReplaceProxyPrefix(OpenApiDocument swaggerDoc, HttpRequest request)
    {
        string proxyPrefix = request.GetReverseProxyPrefix();
        if (proxyPrefix == string.Empty)
        {
            return;
        }

        var paths = new OpenApiPaths();
        foreach (var path in swaggerDoc.Paths)
        {
            paths.Add(path.Key.Insert(0, proxyPrefix), path.Value);

        }
        swaggerDoc.Paths = paths;
    }
}