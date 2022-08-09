using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Common.Metrics;
using inacs.v8.nuget.DevAttributes;

namespace Test.Api.BackgroundServices;
#warning delete me if not needed as this is an example (scoped dependency is consumed)

/// <summary>
/// An example background task
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ExampleBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Constructor for the background task.
    /// The Background Service is a singleton. It accepts the service provider, as it may need scoped dependencies.
    /// </summary>
    /// <param name="serviceProvider">The service provider is used to inject scoped dependencies</param>
    public ExampleBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// This method will be executed on the startup of the program.
    /// The example shows how a scoped dependency is consumed.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ILogger<ExampleBackgroundService> logger = 
            scope.ServiceProvider.GetRequiredService<ILogger<ExampleBackgroundService>>();
        IMetrics metrics = 
            scope.ServiceProvider.GetRequiredService<IMetrics>();
        logger.LogInformation("Hello there! I am started here for a background task");

        _ = Task.Run(async () =>
        {
            var random = new Random();
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                await Task.Delay(1000, stoppingToken);
                metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter, random.Next(100));
            }
        }, stoppingToken);
        return Task.CompletedTask;
    }
}