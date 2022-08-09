using System;
using System.Threading;
using System.Threading.Tasks;
using Config.Middlewares;
using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Test.Api.BackgroundServices;

/// <summary>
/// A hosted service, counting incoming and completed requests and waiting for all requests to finish
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class LifetimeEventsHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly RequestsTracker _requestsTracker;
    private readonly ILogger<LifetimeEventsHostedService> _logger;
        
    /// <summary>
    /// Public constructor of object
    /// </summary>
    /// <param name="hostApplicationLifetime"></param>
    /// <param name="requestsTracker"></param>
    /// <param name="logger"></param>
    public LifetimeEventsHostedService(IHostApplicationLifetime hostApplicationLifetime, RequestsTracker requestsTracker, ILogger<LifetimeEventsHostedService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _requestsTracker = requestsTracker;
        _logger = logger;
    }

    /// <summary>
    /// Registers the on stopping listener
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

        return Task.CompletedTask;
    }
        
    /// <summary>
    /// Waits for all the requests to be finished before stopping the application
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void OnStopping()
    {
        _logger.LogInformation("Blocking all incoming requests");
        _requestsTracker.ShouldRefuseRequests = true;

        _logger.LogInformation("Awaiting all ongoing requests to be processed");
        while (_requestsTracker.OngoingRequestsCount > 0) Thread.Sleep(TimeSpan.FromSeconds(1));

        _logger.LogInformation("All ongoing requests have been processed");

        int briefDelayInSeconds = 5;
        _logger.LogInformation("Initiating a brief delay of {Delay} seconds before shutting down", briefDelayInSeconds);
        while (briefDelayInSeconds >= 0)
        {
            _logger.LogInformation("Shutting down in {Delay} seconds", briefDelayInSeconds);
            briefDelayInSeconds--;
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}