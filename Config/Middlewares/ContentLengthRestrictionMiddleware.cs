using System.Net.Mime;
using System.Threading.Tasks;
using Common.Configurations;
using Common.Resources;
using Config.Extensions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Config.Middlewares;

/// <summary>
/// Middleware for limiting the content lenght of a middleware
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ContentLengthRestrictionMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="nextRequestDelegate"></param>
    public ContentLengthRestrictionMiddleware(RequestDelegate nextRequestDelegate)
    {
        _requestDelegate = nextRequestDelegate;
    }

    /// <summary>
    /// Invocation of the middleware
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="requestState"></param>
    /// <param name="configuration"></param>
    /// <param name="applicationConfig"></param>
    public async Task InvokeAsync(HttpContext context,
        ILogger<ContentLengthRestrictionMiddleware> logger,
        RequestState requestState,
        IConfiguration configuration,
        IOptionsMonitor<ApplicationConfiguration> applicationConfig)
    {
        var configValue = applicationConfig.CurrentValue;

        if (context.Request.ContentLength is not null &&
            context.Request.ContentLength > configValue.ContentLengthRestriction)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;

            ErrorInfo errorInfo = new(Messages.ContentLengthLimit, ErrorDomain.ClientError);
            Error error = new(Messages.ContentLengthLimit, (int)ErrorDomain.ClientError, errorInfo);

            requestState.StopTimer();

            logger.LogInformation("Content length {ContentLength} over limit", context.Request.ContentLength);

            _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
            ResponseContent response =
                ErrorResponseHelper.GenerateErrorResponseContent(context, requestState, error, instanceId,
                    configValue);

            string responseAsJson = ErrorResponseHelper.Convert(response);
            await context.Response.WriteAsync(responseAsJson);
            return;
        }

        await _requestDelegate.Invoke(context);
    }
}