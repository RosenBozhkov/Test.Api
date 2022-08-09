using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;

namespace Config.Middlewares;

/// <summary>
/// Middleware that attaches the request Id
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class AttachRequestIdMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    /// <summary>
    /// Constructor for middleware
    /// </summary>
    /// <param name="nextRequestDelegate"></param>
    public AttachRequestIdMiddleware(RequestDelegate nextRequestDelegate)
    {
        _requestDelegate = nextRequestDelegate;
    }

    /// <summary>
    /// Middleware invocation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="requestState"></param>
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<AttachRequestIdMiddleware> logger,
        RequestState requestState)
    {
        //There is a system RequestId that we override with ours. This scope is needed to override the system one, or
        //in some places there will be our request id and in other - the system one.
        //You do not need to dispose this. ASP.NET Core does it properly and there are no memory leaks - the same memory is used with and without it.
        //It is not disposed as it covers a bigger scope
        logger.BeginScope(new Dictionary<string, object>
        {
            [nameof(requestState.RequestId)] = requestState.RequestId
        });
        logger.LogInformation("Attached request id {RequestId} to request", requestState.RequestId);
        await _requestDelegate.Invoke(context);
    }
}