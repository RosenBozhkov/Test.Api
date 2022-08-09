using Common.Resources;
using Config.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mime;
using Common.Configurations;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.Extensions.Options;

namespace Config.Middlewares;

/// <summary>
/// Global error handling middleware. Catches all exception from the request pipeline.
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class GlobalErrorHandlingMiddleware
{
    /// <summary>
    /// Returns the action to register the global error handling middleware.
    /// </summary>
    /// <returns></returns>
    public static Action<IApplicationBuilder> Configure()
    {
        return configure =>
        {
            configure.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                ErrorInfo errorInfo = new(Messages.GeneralErrorMessage, ErrorDomain.GeneralError);
                Error error = new(Messages.GeneralErrorMessage, (int)ErrorDomain.ClientError, errorInfo);

                RequestState requestState = context.RequestServices.GetRequiredService<RequestState>();
                var applicationConfig =
                    context.RequestServices.GetRequiredService<IOptionsMonitor<ApplicationConfiguration>>();
                requestState.StopTimer();

                ILogger<GlobalErrorHandlingMiddleware> logger =
                    context.CreateLogger<GlobalErrorHandlingMiddleware>(requestState.RequestId.ToString());
                logger.LogError("An unhandled error has occurred for request");

                _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);

                ResponseContent response =
                    ErrorResponseHelper.GenerateErrorResponseContent(context, requestState, error, instanceId,
                        applicationConfig.CurrentValue);

                string responseAsJson = ErrorResponseHelper.Convert(response);
                await context.Response.WriteAsync(responseAsJson);
            });
        };
    }
}