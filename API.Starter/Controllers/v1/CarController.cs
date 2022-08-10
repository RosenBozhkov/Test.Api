using System;
using System.Collections.Generic;
using Business.Interfaces.v1;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Config.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Metrics;
using Common.Metrics;

namespace Test.Api.Controllers.v1;

/// <summary>
/// Controller for managing cars
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarController : ControllerBase
{
    private static readonly string ControllerName = typeof(CarController).FullName!;
    private readonly ICarService _carService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="carService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public CarController(
        ICarService carService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _carService = carService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting all the cars
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<CarResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IList<CarResponse>>> GetAllAsync()
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetAllAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IList<CarResponse> cars = await _carService.GetAllAsync();

        return new ResponseContent<IList<CarResponse>>
        {
            Result = cars
        };
    }

    /// <summary>
    /// Endpoint for getting a car by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Journal]
    [Anonymous]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<CarResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<CarResponse>> GetByIdAsync(Guid id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetByIdAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        CarResponse car = await _carService.GetResponseByIdAsync(id);

        return new ResponseContent<CarResponse>
        {
            Result = car
        };
    }

    /// <summary>
    /// Endpoint for creating Car
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    // [Authorization("ADMIN", "ROOT", "LEAF", "WOOD")]
    [Anonymous]
    [HttpPost]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<CarResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    // [HideRequestBody]
    // [HideResponseBody]
    public async Task<ResponseContent<CarResponse>> CreateAsync(CarCreateRequest car)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        CarResponse created = await _carService.CreateAsync(car);

        return new ResponseContent<CarResponse>
        {
            Result = created
        };
    }

    /// <summary>
    /// Endpoint for updating a car by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="car"></param>
    /// <returns></returns>
    [Anonymous]
    [HttpPut("{id:guid}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<CarResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<CarResponse>> UpdateAsync(Guid id, CarCreateRequest car)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(UpdateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        CarResponse updated = await _carService.UpdateAsync(id, car);

        return new ResponseContent<CarResponse>
        {
            Result = updated
        };
    }

    /// <summary>
    /// Endpoint for deleting a car by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Anonymous]
    [Obsolete]
    [HttpDelete("{id:guid}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent> DeleteAsync(Guid id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(DeleteAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        await _carService.DeleteAsync(id);

        return new ResponseContent();
    }
}