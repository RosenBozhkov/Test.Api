using System;
using System.Collections.Generic;
using Business.Interfaces.v1;
using inacs.v8.nuget.DevAttributes;
using Config.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Metrics;
using Common.Metrics;
using Business.Models.v1;

namespace Test.Api.Controllers.v1;

/// <summary>
/// Controller for managing visits
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class VisitController
{
    private static readonly string ControllerName = typeof(VisitController).FullName!;
    private readonly IVisitService _visitService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="visitService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public VisitController(
        IVisitService visitService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _visitService = visitService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting all the visits
    /// </summary>
    [Anonymous]
    [HttpGet]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<VisitResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IList<VisitResponse>>> GetAllAsync()
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetAllAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IList<VisitResponse> visits = await _visitService.GetAllAsync();

        return new ResponseContent<IList<VisitResponse>>
        {
            Result = visits
        };
    }

    /// <summary>
    /// Endpoint for getting a visit by Id
    /// </summary>
    /// <param name="id"></param>
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Journal]
    [Anonymous]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<VisitResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<VisitResponse>> GetByIdAsync(Guid id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetByIdAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        VisitResponse visit = await _visitService.GetByIdAsync(id);

        return new ResponseContent<VisitResponse>
        {
            Result = visit
        };
    }

    /// <summary>
    /// Endpoint for registering a Visit for a Car
    /// </summary>
    /// <param name="visit"></param>
    // [Authorization("ADMIN", "ROOT", "LEAF", "WOOD")]
    [Anonymous]
    [HttpPost]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<VisitResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    // [HideRequestBody]
    // [HideResponseBody]
    public async Task<ResponseContent<VisitResponse>> RegisterCarAsync(VisitRegisterRequest visit)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        VisitResponse created = await _visitService.CreateAsync(visit);

        return new ResponseContent<VisitResponse>
        {
            Result = created
        };
    }

    /// <summary>
    /// Endpoint for finalizing a visit by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="visit"></param>
    [Anonymous]
    [HttpPut("{id:guid}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<VisitResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<VisitResponse>> ReturnCarAsync(Guid id, VisitFinishRequest visit)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(UpdateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        VisitResponse updated = await _visitService.UpdateAsync(id, visit);

        return new ResponseContent<VisitResponse>
        {
            Result = updated
        };
    }

    /// <summary>
    /// Endpoint for deleting a visit by Id
    /// </summary>
    /// <param name="id"></param>
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

        await _visitService.DeleteAsync(id);

        return new ResponseContent();
    }
}
