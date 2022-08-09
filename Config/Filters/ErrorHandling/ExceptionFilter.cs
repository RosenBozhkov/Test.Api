using inacs.v8.nuget.DevAttributes;
using Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using App.Metrics;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.EnvHelper;
using inacs.v8.nuget.ExceptionCollector;
using inacs.v8.nuget.ExposeDeveloper.Util;
using inacs.v8.nuget.IdentityManager.Rules;
using inacs.v8.nuget.Metrics.Common;

namespace Config.Filters.ErrorHandling;

/// <summary>
/// Filter for handling exceptions
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ExceptionFilter : IAsyncExceptionFilter
{
    private const string StateMachineMethodName = "MoveNext";
    private const char MethodStartDelimiter = '<';
    private const char MethodEndDelimiter = '>';
    private const int MethodNameIndex = 1;
    private readonly IExceptionCollector _exceptionCollector;
    private readonly IMeasureMetrics _metrics;
    private readonly ILogger<ExceptionFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="exceptionCollector"></param>
    /// <param name="metrics"></param>
    /// <param name="logger"></param>
    public ExceptionFilter(IExceptionCollector exceptionCollector, IMeasureMetrics metrics, ILogger<ExceptionFilter> logger)
    {
        _exceptionCollector = exceptionCollector;
        _metrics = metrics;
        _logger = logger;
    }

    /// <summary>
    /// Handling the exception
    /// </summary>
    /// <param name="context"></param>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is InternalException exception)
        {
            context.HttpContext.Response.StatusCode = (int)exception.StatusCode;
        }
        else
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        Error error = context.Exception switch
        {
            RuleNotFoundException ex => GenerateError(ex),
            EnvironmentNotConfiguredException ex => GenerateError(ex),
            InternalException ex => GenerateInternalError(ex),
            null => GenerateError(),
            Exception ex => GenerateGeneralError(ex)
        };

        ResponseContent response = new()
        {
            Error = error
        };
        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;

        await CollectExceptionAsync(context.Exception);

        _metrics.Counter.Increment(HttpRegistry.ExceptionFilterCounter);
    }

    private async Task CollectExceptionAsync(Exception? exception)
    {
        if (exception is null)
        {
            return;
        }

        MethodBase? method = exception.TargetSite;
        if (method is null)
        {
            _logger.LogCritical("Exception without target site!");
            return;
        }

        GetDeveloperInformationFromMethod(
            method,
            out Type type,
            out List<DeveloperInformation> developerInformation
        );

        if (!developerInformation.Any())
        {
            developerInformation = type.GetDeveloperInformation();
        }

        if (!developerInformation.Any())
        {
            developerInformation = type.Assembly.GetDeveloperInformation();
        }

        if (!developerInformation.Any())
        {
            developerInformation = Assembly.GetExecutingAssembly().GetDeveloperInformation();
        }

        await _exceptionCollector.CollectExceptionAsync(exception, developerInformation);
    }

    private static void GetDeveloperInformationFromMethod(MethodBase method,
        out Type type,
        out List<DeveloperInformation> developerInformation)
    {
        if (method.Name == StateMachineMethodName)
        {
            type = method.ReflectedType!.ReflectedType!;
            string methodName = method.ReflectedType.Name
                .Split(MethodStartDelimiter, MethodEndDelimiter)[MethodNameIndex];
            developerInformation = type.GetMethods()
                .Where(m => m.Name == methodName)
                .SelectMany(m => m.GetDeveloperInformation())
                .ToList();
        }
        else
        {
            type = method.ReflectedType!;
            developerInformation = method.GetDeveloperInformation();
        }
    }

    private Error GenerateError(EnvironmentNotConfiguredException ex)
    {
        _logger.LogError(ex, "Environment variable {Variable} not set", ex.EnvironmentVariable);
        ErrorInfo errorInfo = new(ex.Message, ErrorDomain.ServerError);
        return new Error(ex.Message, (int)ErrorDomain.ServerError, errorInfo);
    }

    private Error GenerateError(RuleNotFoundException ex)
    {
        _logger.LogError(ex, "Rule not found");
        ErrorInfo errorInfo = new(Messages.GeneralErrorMessage, ex.ErrorDomain);
        return new Error(Messages.GeneralErrorMessage, ex.ErrorCode, errorInfo);
    }

    private Error GenerateError()
    {
        _logger.LogError("General exception occured with and exception was null");
        ErrorInfo errorInfo = new(Messages.GeneralErrorMessage, ErrorDomain.GeneralError);
        return new Error(Messages.GeneralErrorMessage, (int)ErrorDomain.GeneralError, errorInfo);
    }
    private Error GenerateGeneralError(Exception ex)
    {
        _logger.LogError(ex, "General exception occured with {Message}", ex.Message);
        ErrorInfo errorInfo = new(Messages.GeneralErrorMessage, ErrorDomain.GeneralError);
        return new Error(Messages.GeneralErrorMessage, (int)ErrorDomain.GeneralError, errorInfo);
    }

    private Error GenerateInternalError(InternalException ex)
    {
        _logger.LogError(ex, "Exception from domain {Domain} with message {Message} occured", ex.ErrorDomain,
            ex.Message);
        ErrorInfo errorInfo = new(ex.Message, ex.ErrorDomain);
        return new Error(ex.Message, ex.ErrorCode, errorInfo);
    }
}