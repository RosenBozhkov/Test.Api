using System.Linq;
using Config.Extensions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Config.Filters.Result;

/// <summary>
/// Filter that logs each response
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class LogResponseFilter : IAlwaysRunResultFilter
{
    private readonly RequestState _requestState;
    private readonly ILogger<LogResponseFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public LogResponseFilter(RequestState requestState, ILogger<LogResponseFilter> logger)
    {
        _requestState = requestState;
        _logger = logger;
    }

    /// <summary>
    /// No action needed
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuting(ResultExecutingContext context)
    {
        //No action needed
    }

    /// <summary>
    /// Logs each response
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuted(ResultExecutedContext context)
    {
        object? body = null;
        if (!context.Filters.OfType<HideResponseBodyAttribute>().Any())
        {
            body = (context.Result as ObjectResult)?.Value;
        }

        _logger.LogInformation("{@Response}", new
        {
            Body = body,
            Headers = FiltersHelper.GetResponseHeaders(context.HttpContext),
            _requestState.ExecutionTimeMs,
            context.HttpContext.Response.StatusCode
        });
    }
}