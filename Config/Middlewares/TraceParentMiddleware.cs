using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Common.Configurations;
using Common.Exceptions;
using Common.Extensions;
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
/// Middleware for validation of trade and span id for telemetry
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class TraceParentMiddleware
{
    private const string TraceParentKey = "traceparent";
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public TraceParentMiddleware(RequestDelegate next)
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
        ILogger<TraceParentMiddleware> logger,
        RequestState requestState,
        IOptionsMonitor<ApplicationConfiguration> applicationConfig)
    {
        string traceParent = context.Request.Headers[TraceParentKey].ToString();
        if (!string.IsNullOrEmpty(traceParent))
        {
            try
            {
                string[] tokens = traceParent.Split('-');
                if (tokens.Length != 4) throw new InvalidTraceException(Messages.InvalidTrace);

                string version = tokens[0];
                string traceId = tokens[1];
                string parentId = tokens[2];
                string flags = tokens[3];

                if (traceParent.Length != 55 || traceId.Length != 32 || parentId.Length != 16)
                    throw new InvalidTraceException(Messages.InvalidTrace);
                    
                if (version != "00" || (flags != "00" && flags != "01"))
                    throw new InvalidTraceException(Messages.InvalidTrace);

                bool isTraceIdValidHex = traceId.All(c => c.IsHexadecimal());
                bool isParentIdIdValidHex = parentId.All(c => c.IsHexadecimal());

                if (!isTraceIdValidHex || !isParentIdIdValidHex)
                    throw new InvalidTraceException(Messages.InvalidTrace);
            }
            catch (Exception ex)
            {
                await WriteErrorResponseAsync(context, logger, requestState, applicationConfig, ex);
                return;
            }
        }

        await _next(context);
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, ILogger<TraceParentMiddleware> logger,
        RequestState requestState,
        IOptionsMonitor<ApplicationConfiguration> applicationConfig, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        ErrorInfo errorInfo = new(Messages.InvalidTrace, ErrorDomain.ClientError);
        Error error = new(Messages.InvalidTrace, (int)ErrorDomain.ClientError, errorInfo);

        requestState.StopTimer();

        logger.LogInformation(ex, "Not valid trace id");

        _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
        ResponseContent response = ErrorResponseHelper.GenerateErrorResponseContent(context, requestState,
            error, instanceId, applicationConfig.CurrentValue);

        string responseAsJson = ErrorResponseHelper.Convert(response);
        await context.Response.WriteAsync(responseAsJson);
    }
}