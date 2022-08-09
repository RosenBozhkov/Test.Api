using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Common.Resources;
using inacs.v8.nuget.Core.Exceptions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.IdentityManager.Extensions;
using inacs.v8.nuget.IdentityManager.Interfaces;
using inacs.v8.nuget.Metrics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Config.Filters.Auth;

/// <summary>
/// Filter used to authorize requests
/// </summary>
public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly IIdentityValidatorService _validatorService;
    private readonly RequestState _state;
    private readonly IMeasureMetrics _metrics;
    private readonly ILogger<AuthorizationFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="validatorService"></param>
    /// <param name="state"></param>
    /// <param name="metrics"></param>
    /// <param name="logger"></param>
    public AuthorizationFilter(IIdentityValidatorService validatorService, RequestState state, IMeasureMetrics metrics, ILogger<AuthorizationFilter> logger)
    {
        _validatorService = validatorService;
        _state = state;
        _metrics = metrics;
        _logger = logger;
    }

    /// <summary>
    /// Checks if the request is authorized
    /// </summary>
    /// <param name="context"></param>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            (Principal? principal, string? rightCode) = await _validatorService.ValidateTokenAsync(context);
            _state.Principal = principal;
            _state.ActionRightCode = rightCode;
            _state.IsAllowed = true;
            if (principal is not null)
            {
                string? clientIp = _state.ClientIp;
                _logger.LogInformation("User {Username} with email {Email} from {IpAddress} authorized successfully", principal.Username, principal.Email, clientIp);
            }
        }
        catch (Exception ex)
        {
            _state.IsAllowed = false;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            
            _logger.LogError(ex, "Invalid authentication attempt");
            _metrics.Counter.Increment(HttpRegistry.AuthorizationFailedCounter);
            ErrorInfo errorInfo = new(Messages.UnauthenticatedRequest, ErrorDomain.ClientError);
            int errorCode = ex is InternalException internalException ? internalException.ErrorCode : (int)ErrorDomain.ClientError;
            Error error = new(Messages.UnauthenticatedRequest, errorCode, errorInfo);
            ResponseContent response = new()
            {
                Error = error
            };
            context.Result = new ObjectResult(response);
        }
    }
}