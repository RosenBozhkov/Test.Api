using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Config.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;
using Common.Configurations;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.EnvHelper;

namespace Config.Middlewares;

/// <summary>
/// Middleware for erroneous api version
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ApiVersionErrorResponse : IErrorResponseProvider
{
    private readonly ApplicationConfiguration _applicationConfiguration;

    /// <summary>
    /// Constructor accepting version header key and default api version. They are passed manually in Startup.cs
    /// </summary>
    /// <param name="applicationConfiguration"></param>
    public ApiVersionErrorResponse(ApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }
    /// <summary>
    /// Creates the error response
    /// In the context message, the version is formatted specifically by default. If there is a 0
    /// followed by a digit before a dot, the 0 will be removed.
    /// For example 2.01 is actually 2.1. 2.10 is 2.10 for the version.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IActionResult CreateResponse(ErrorResponseContext context)
    {
        var httpContext = context.Request.HttpContext;
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        
        string requestVersion = httpContext.Request.Headers[_applicationConfiguration.VersionHeaderKey].ToString();
        string message = string.Format(Messages.UnsupportedApiVersion, requestVersion, _applicationConfiguration.DefaultApiVersion);
        ErrorInfo errorInfo = new(message, ErrorDomain.ClientError);
        Error error = new(message, (int)ErrorDomain.ClientError, errorInfo);

        RequestState requestState = httpContext.RequestServices.GetRequiredService<RequestState>();
        requestState.StopTimer();

        ILogger<ApiVersionErrorResponse> logger =
            httpContext.CreateLogger<ApiVersionErrorResponse>(requestState.RequestId.ToString());
        logger.LogError("An unhandled error has occurred for request");

        _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);

        ResponseContent response =
            ErrorResponseHelper.GenerateErrorResponseContent(context.Request.HttpContext, requestState, error, instanceId, _applicationConfiguration);

        return new ObjectResult(response);
    }
}