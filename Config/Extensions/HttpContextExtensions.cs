using System;
using System.Collections.Generic;
using inacs.v8.nuget.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Config.Extensions;

/// <summary>
/// Extension for adding a logger in middleware
/// </summary>
internal static class HttpContextExtensions
{
    /// <summary>
    /// Creates a logger and adds a request id to it
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="requestId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ILogger<T> CreateLogger<T>(this HttpContext httpContext, string requestId)
    {
        ILogger<T> logger = httpContext.RequestServices.GetRequiredService<ILogger<T>>();
        //You do not need to dispose this. ASP.NET Core does it properly and there are no memory leaks - the same memory is used with and without it.
        //It is not disposed as it covers a bigger scope
        logger.BeginScope(new Dictionary<string, object>
        {
            [nameof(RequestState.RequestId)] = requestId
        });

        return logger;
    }
}