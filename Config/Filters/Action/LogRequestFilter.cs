using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Config.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Config.Filters.Action;

/// <summary>
/// A filter that logs each request
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class LogRequestFilter : IActionFilter
{
    private readonly ILogger<LogRequestFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="logger"></param>
    public LogRequestFilter(ILogger<LogRequestFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs each request
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        string ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? Constants.RemoteIpAddressNotFound;
        string httpVerb = context.HttpContext.Request.Method;
        string method = context.HttpContext.Request.Method;
        string? body = null;

        try
        {
            if (!context.Filters.OfType<HideRequestBodyAttribute>().Any())
            {
                body = FiltersHelper.GetRequestBodyAsync(context.HttpContext).Result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error getting request body");
            body = string.Join(";", context.ActionArguments.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        _logger.LogInformation("{@Request}", new
        {
            IP = ip,
            HTTPVerb = httpVerb,
            Url = $"{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}",
            Route = context.ActionDescriptor.DisplayName,
            Body = body,
            Method = method,
            Headers = FiltersHelper.GetRequestHeaders(context.HttpContext)
        });
    }

    /// <summary>
    /// No action
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //No action needed
    }
}