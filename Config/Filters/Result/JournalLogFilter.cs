using System;
using System.Collections.Generic;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.IdentityManager.Extensions;
using inacs.v8.nuget.JournalLogs.Interfaces;
using inacs.v8.nuget.JournalLogs.Persistence.Models;

namespace Config.Filters.Result;

/// <summary>
/// Journal filter triggered when a controller is annotated with the <see cref="JournalAttribute"/>.
/// An action is considered successful only when no exception is thrown (meaning exception is null)
/// and when the status code is 2xx
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class JournalLogFilter : IAsyncAlwaysRunResultFilter
{
    private readonly RequestState _requestState;
    private readonly IJournalCollector _journalCollector;
    private readonly ILogger<JournalLogFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="requestState"></param>
    /// <param name="journalCollector"></param>
    /// <param name="logger"></param>
    public JournalLogFilter(RequestState requestState, IJournalCollector journalCollector, ILogger<JournalLogFilter> logger)
    {
        _requestState = requestState;
        _journalCollector = journalCollector;
        _logger = logger;
    }

    /// <summary>
    /// Inserts the journal information for this request
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next.Invoke();

        if (!context.Filters.OfType<JournalAttribute>().Any()) return;

        try
        {
            Principal? principal = _requestState.Principal;
            List<string> rights = (principal?.GetAuthRights() ?? new List<PrincipalRight>())
                .Select(r => r.Name)
                .ToList();
            await _journalCollector.CollectJournalAsync(new JournalEntry
            {
                UserName = principal?.Username,
                RightCode = _requestState.ActionRightCode,
                IsAllowed = _requestState.IsAllowed,
                Rights = string.Join(", ", rights),
                StatusCode = context.HttpContext.Response.StatusCode,
                Method = context.ActionDescriptor.DisplayName,
                ClientIp = _requestState.ClientIp,
                ClientDomain = _requestState.ClientDomain
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during journal collection");
            throw;
        }
    }
}