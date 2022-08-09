using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Config.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Threading.Tasks;
using Common.Configurations;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.EnvHelper;
using Microsoft.Extensions.Options;

namespace Config.Middlewares;

/// <summary>
/// Middleware that counts requests and responses. On shutdown, it blocks new requests.
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class GracefulMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestsTracker _requestsTracker;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="requestsTracker"></param>
    public GracefulMiddleware(RequestDelegate next, RequestsTracker requestsTracker)
    {
        _next = next;
        _requestsTracker = requestsTracker;
    }

    /// <summary>
    /// Invocation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="requestState"></param>
    /// <param name="applicationConfig"></param>
    public async Task InvokeAsync(HttpContext context, ILogger<GracefulMiddleware> logger, RequestState requestState, IOptionsMonitor<ApplicationConfiguration> applicationConfig)
    {
        if (_requestsTracker.ShouldRefuseRequests)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            ErrorInfo errorInfo = new(Messages.ResourceNotFound, ErrorDomain.ClientError);
            Error error = new(Messages.ResourceNotFound, (int)ErrorDomain.ClientError, errorInfo);

            requestState.StopTimer();

            logger.LogDebug("Resource not found for request");

            _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
            Meta meta = new();
            meta.Initialize(requestState, context, instanceId, applicationConfig.CurrentValue);

            ResponseContent response = new()
            {
                Error = error,
                Meta = meta
            };

            string responseAsJson = ErrorResponseHelper.Convert(response);
            await context.Response.WriteAsync(responseAsJson);

            return;
        }

        _requestsTracker.Increment();

        await _next(context).ConfigureAwait(false);

        _requestsTracker.Decrement();
    }
}