using System;
using System.Linq;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Config.Filters.Action;

/// <summary>
/// A filter that adds a warning to the response if a method is obsolete
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ObsoleteWarningFilter : IActionFilter
{
    private readonly ILogger<ObsoleteWarningFilter> _logger;
    private readonly RequestState _request;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="request"></param>
    public ObsoleteWarningFilter(ILogger<ObsoleteWarningFilter> logger, RequestState request)
    {
        _logger = logger;
        _request = request;
    }

    /// <summary>
    /// Adds a warning to the response if endpoint is obsolete
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var obsoleteAttributes = context.ActionDescriptor.EndpointMetadata
            .OfType<ObsoleteAttribute>()
            .ToList();

        if (!obsoleteAttributes.Any())
        {
            return;
        }
            
        foreach (var obsoleteAttribute in obsoleteAttributes)
        {
            _request.Errors.Add(
                new ErrorInfo(obsoleteAttribute.Message ?? "Endpoint is obsolete", ErrorDomain.ClientError, ErrorType.Warning));
        }
            
        _logger.LogInformation("Added {ObsoleteWarningsCount} obsolete warnings", obsoleteAttributes.Count);
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