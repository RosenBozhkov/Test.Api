using System.Linq;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Config.Filters.Result;

/// <summary>
/// Filter for attaching warnings to response if present
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ErrorInfoFilter : IAlwaysRunResultFilter
{
    private readonly RequestState _requestState;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="requestState"></param>
    public ErrorInfoFilter(RequestState requestState)
    {
        _requestState = requestState;
    }
    /// <summary>
    /// Adds the warnings to the errors. If the Error object is null (returned null from controller), then initialize it.
    /// If the Error object is initialized, either by the controller or by the ExceptionFilter, then just add
    /// the additional ErrorInfo to the list.
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!_requestState.Errors.Any())
        {
            return;
        }

        object response = ((ObjectResult)context.Result).Value!;
        ResponseContent responseContent = (ResponseContent)response;

        responseContent.Error ??= new Error(string.Empty, (int)ErrorDomain.GeneralError);

        foreach (var error in _requestState.Errors)
        {
            responseContent.Error.Errors.Add(error);
        }
    }

    /// <summary>
    /// No action
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuted(ResultExecutedContext context)
    {
        //No action needed
    }
}