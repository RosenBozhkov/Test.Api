using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Metrics;
using Common.Metrics;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.ServiceDiscovery.Contracts;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Controllers.v1;

namespace Test.Api.Controllers.MaybeCheckLater;

/// <summary>
/// Example service discovery
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class ServiceDiscoveryController : ControllerBase
{
    private static readonly string ControllerName = typeof(CarController).FullName!;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;
    private readonly IServiceUriProvider _serviceProvider;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public ServiceDiscoveryController(
        IServiceUriProvider serviceProvider,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _serviceProvider = serviceProvider;
        _telemetryProvider = telemetryProvider;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting random uri of a service by its name.
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet("serviceUri/getRandom")]
    [Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<string>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<Uri?>> GetUriAsync(string serviceName)
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetUriAsync));
        _metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        var serviceUri = await _serviceProvider.GetServiceUri(serviceName);

        return new ResponseContent<Uri?>()
        {
            Result = serviceUri
        };
    }

    /// <summary>
    /// Endpoint for getting the uri of the first responding instance of a given service by its name.
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet("serviceUri/getFastest")]
    [Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<string>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<Uri?>> GetFastestUriAsync(string serviceName)
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetFastestUriAsync));
        _metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        var serviceUri = await _serviceProvider.GetFirstRespondingServiceUri(serviceName);

        return new ResponseContent<Uri?>()
        {
            Result = serviceUri
        };
    }
    /// <summary>
    /// Endpoint for getting all uris of a given service by its name.
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet("serviceUri/fetchAll")]
    [Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<IEnumerable<Uri>?>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IEnumerable<Uri>?>> FetchUrisAsync(string serviceName)
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(FetchUrisAsync));
        _metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IEnumerable<Uri>? serviceUri = await _serviceProvider.FetchServiceUris(serviceName);

        return new ResponseContent<IEnumerable<Uri>?>()
        {
            Result = serviceUri
        };
    }

    /// <summary>
    /// Endpoint for getting all uris that respond of a given service by its name.
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet("serviceUri/fetchAlive")]
    [Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<IEnumerable<Uri?>>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IEnumerable<Uri?>>> FetchAliveAsync(string serviceName)
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(FetchAliveAsync));
        _metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IEnumerable<Uri?> cars = await _serviceProvider.FetchHealthCheckedServiceUris(serviceName);

        return new ResponseContent<IEnumerable<Uri?>>
        {
            Result = cars
        };
    }
}