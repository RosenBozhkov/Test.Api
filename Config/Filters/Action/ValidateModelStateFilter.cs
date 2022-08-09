using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using inacs.v8.nuget.Core.Models;

namespace Config.Filters.Action;

/// <summary>
/// Filter that validates the model state of the object
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ValidateModelStateFilter : IActionFilter
{
    /// <summary>
    /// Validates the request body
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            ErrorInfo[] errors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => new ErrorInfo(x.ErrorMessage, ErrorDomain.ClientError))
                .ToArray();
            string message = string.Join(Constants.ErrorDelimiter, errors.Select(e => e.Message));


            ResponseContent response = new()
            {
                Error = new Error(message, (int)ErrorDomain.ClientError, errors)
            };

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new ObjectResult(response);
        }
    }

    /// <summary>
    /// No action needed
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //No action needed
    }
}