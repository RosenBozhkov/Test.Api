using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Interfaces.v1;
using Test.Api.BackgroundServices;

namespace API.Starter.Tests.Extensions;

public static class TestExtensions
{
    public static void RemoveHostedServices(this IServiceCollection collection)
    {
        //Remove all hosted services from the Exe dll. They are not needed for this kind of test
        var hostedServices = collection
            .Where(descriptor => descriptor.ServiceType == typeof(IHostedService) &&
                                 descriptor.ImplementationType?.Assembly == typeof(LifetimeEventsHostedService).Assembly)
            .ToList();
        foreach (var service in hostedServices)
        {
            collection.Remove(service);
        }
    }
    
    public static void InjectMock(this IServiceCollection serviceCollection, ICarRepository carRepository)
    {
        var descriptor =
            serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(ICarRepository));
        serviceCollection.Remove(descriptor);
        serviceCollection.AddScoped(_ => carRepository);
    }

    public static void ResetMetrics()
    {
        var field = typeof(MetricsAspNetHostBuilderExtensions).GetField("_metricsBuilt", 
            BindingFlags.Static | 
            BindingFlags.NonPublic);

        // Normally the first argument to "SetValue" is the instance
        // of the type but since we are mutating a static field we pass "null"
        field!.SetValue(null, false);
    }
}