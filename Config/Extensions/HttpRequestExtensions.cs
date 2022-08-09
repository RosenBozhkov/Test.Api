using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Http;

namespace Config.Extensions;

[Developer("Vasil Egov", "v.egov@itsoft.bg")]
internal static class HttpRequestExtensions
{
    /// <summary>
    /// Returns the proxy prefix by extracting it from the x-Referer header.
    /// This header is set in the reverse proxy on each request with the following command.
    /// RequestHeader set X-Referer %{REQUEST_URI}s
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Returns the proxy prefix with a leading forward slash and no following forward slash if header is set, else returns empty string.</returns>
    internal static string GetReverseProxyPrefix(this HttpRequest request)
    {
        string proxyPrefix = request.Headers["x-Referer"].ToString();
        if (!string.IsNullOrEmpty(proxyPrefix))
        {
            proxyPrefix = proxyPrefix.Replace(request.Path, string.Empty);
        }

        return proxyPrefix ?? string.Empty;
    }
}