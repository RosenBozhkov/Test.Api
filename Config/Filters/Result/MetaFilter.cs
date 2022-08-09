using Common.Configurations;
using inacs.v8.nuget.DevAttributes;
using Config.Extensions;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.EnvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Config.Filters.Result;

/// <summary>
/// Filter that attaches the meta to each request
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class MetaFilter : IAlwaysRunResultFilter
{
    private readonly RequestState _requestState;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ILogger<MetaFilter> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="requestState"></param>
    /// <param name="applicationConfig"></param>
    /// <param name="logger"></param>
    public MetaFilter(RequestState requestState, 
        IOptionsMonitor<ApplicationConfiguration> applicationConfig,
        ILogger<MetaFilter> logger)
    {
        _requestState = requestState;
        _applicationConfiguration = applicationConfig.CurrentValue;
        _logger = logger;
    }

    /// <summary>
    /// Attaches meta to each request
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuting(ResultExecutingContext context)
    {
        _requestState.StopTimer();
        if (context.Result is ObjectResult result)
        {
            _ = int.TryParse(EnvironmentHelper.InstanceId, out int instanceId);
            Meta meta = new();
            meta.Initialize(_requestState, context.HttpContext, instanceId, _applicationConfiguration);

            object response = result.Value!;
            ((ResponseContent)response).Meta = meta;
        }

        _logger.LogInformation("Finished executing {Method}", context.ActionDescriptor.DisplayName);
    }

    /// <summary>
    /// No action needed
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuted(ResultExecutedContext context)
    {
        //No action needed
    }
}