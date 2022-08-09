using System.Threading.Tasks;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.IdentityManager.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Config.Middlewares;

/// <summary>
/// Middleware that logs request ip addresses
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class LogClientIpMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    /// <summary>
    /// Constructor for middleware
    /// </summary>
    /// <param name="nextRequestDelegate"></param>
    public LogClientIpMiddleware(RequestDelegate nextRequestDelegate)
    {
        _requestDelegate = nextRequestDelegate;
    }

    /// <summary>
    /// Middleware invocation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<LogClientIpMiddleware> logger)
    {
        string clientIp = context.Request.GetClientIp();
        logger.LogInformation("Client ip address {IpAddress} requested {Url}", clientIp, context.Request.GetDisplayUrl());
        await _requestDelegate.Invoke(context);
    }
}