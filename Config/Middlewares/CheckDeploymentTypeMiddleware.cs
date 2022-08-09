using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Common.Configurations;
using Common.Resources;
using Config.Extensions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Config.Middlewares;

/// <summary>
/// Middleware that checks for the deployment type of the client and service
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class CheckDeploymentTypeMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    /// <summary>
    /// Constructor for middleware
    /// </summary>
    /// <param name="nextRequestDelegate"></param>
    public CheckDeploymentTypeMiddleware(RequestDelegate nextRequestDelegate)
    {
        _requestDelegate = nextRequestDelegate;
    }

    /// <summary>
    /// Middleware invocation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="requestState"></param>
    /// <param name="applicationConfig"></param>
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<CheckDeploymentTypeMiddleware> logger,
        RequestState requestState,
        IOptionsMonitor<ApplicationConfiguration> applicationConfig)
    {
        string deploymentType = context.Request.Headers["ITSoft-deployment-type"].ToString();

        if (!string.IsNullOrEmpty(deploymentType) && deploymentType != EnvironmentHelper.DeploymentType)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ErrorInfo errorInfo = new(Messages.InvalidDeploymentType, ErrorDomain.ClientError);
            Error error = new(Messages.InvalidDeploymentType, (int)ErrorDomain.ClientError, errorInfo);

            requestState.StopTimer();

            logger.LogInformation("Client deployment type {ClientDeploymentType} is not {DeploymentType}", deploymentType, EnvironmentHelper.DeploymentType);

            _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
            ResponseContent response =
                ErrorResponseHelper.GenerateErrorResponseContent(context, requestState, error, instanceId,
                    applicationConfig.CurrentValue);

            string responseAsJson = ErrorResponseHelper.Convert(response);
            await context.Response.WriteAsync(responseAsJson);
            return;
        }

        await _requestDelegate.Invoke(context);
    }
}