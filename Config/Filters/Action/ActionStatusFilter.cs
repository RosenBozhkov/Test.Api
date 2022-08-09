using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Config.Filters.Action;

/// <summary>
/// Filter for setting an explicit http status code
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ActionStatusFilter : IActionFilter
{
    private readonly RequestState _requestState;
    private readonly ILogger<ActionStatusFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public ActionStatusFilter(RequestState requestState, ILogger<ActionStatusFilter> logger)
    {
        _logger = logger;
        _requestState = requestState;
    }

    /// <summary>
    /// No action needed
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        //No action needed
    }

    /// <summary>
    /// Sets the specified http status code
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is null && _requestState.IsStatusCodeSet)
        {
            _logger.LogInformation("Set status code to {StatusCode}", _requestState.StatusCode);
            context.HttpContext.Response.StatusCode = (int)_requestState.StatusCode;
        }
    }
}