using System.Net;
using Common.Resources;
using Config.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Threading.Tasks;
using Common.Configurations;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.Extensions.Options;

namespace Config.Middlewares;

/// <summary>
/// Middleware for handling 404 responses
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public NotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invocation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="requestState"></param>
    /// <param name="applicationConfig"></param>
    public async Task InvokeAsync(HttpContext context,
        ILogger<NotFoundMiddleware> logger,
        RequestState requestState,
        IOptionsMonitor<ApplicationConfiguration> applicationConfig)
    {
        await _next(context);
        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            ErrorInfo errorInfo = new(Messages.ResourceNotFound, ErrorDomain.ClientError);
            Error error = new(Messages.ResourceNotFound, (int)ErrorDomain.ClientError, errorInfo);

            requestState.StopTimer();

            logger.LogInformation("Resource not found for request");

            _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
            ResponseContent response = ErrorResponseHelper.GenerateErrorResponseContent(context, requestState,
                error, instanceId, applicationConfig.CurrentValue);

            string responseAsJson = ErrorResponseHelper.Convert(response);
            await context.Response.WriteAsync(responseAsJson);
        }
    }
}