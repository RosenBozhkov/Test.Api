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
/// Controller for managing makes
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class MakeController : ControllerBase
{
    private static readonly string ControllerName = typeof(MakeController).FullName!;
    private readonly IMakeService _makeService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="makeService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public MakeController(
        IMakeService makeService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _makeService = makeService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting all the makes
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<MakeResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IList<MakeResponse>>> GetAllAsync()
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetAllAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IList<MakeResponse> makes = await _makeService.GetAllAsync();

        return new ResponseContent<IList<MakeResponse>>
        {
            Result = makes
        };
    }

    /// <summary>
    /// Endpoint for getting a make by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Journal]
    [Anonymous]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<MakeResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<MakeResponse>> GetByIdAsync(Guid id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetByIdAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        MakeResponse make = await _makeService.GetByIdAsync(id);

        return new ResponseContent<MakeResponse>
        {
            Result = make
        };
    }

    /// <summary>
    /// Endpoint for creating Make
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    // [Authorization("ADMIN", "ROOT", "LEAF", "WOOD")]
    [Anonymous]
    [HttpPost]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<MakeResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    // [HideRequestBody]
    // [HideResponseBody]
    public async Task<ResponseContent<MakeResponse>> CreateAsync(MakeRequest model)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        MakeResponse created = await _makeService.CreateAsync(model);

        return new ResponseContent<MakeResponse>
        {
            Result = created
        };
    }

    /// <summary>
    /// Endpoint for updating a make by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="make"></param>
    /// <returns></returns>
    [Anonymous]
    [HttpPut("{id:guid}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<MakeResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<MakeResponse>> UpdateAsync(Guid id, MakeRequest model)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(UpdateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        MakeResponse updated = await _makeService.UpdateAsync(id, model);

        return new ResponseContent<MakeResponse>
        {
            Result = updated
        };
    }

    /// <summary>
    /// Endpoint for deleting a make by Id
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

        await _makeService.DeleteAsync(id);

        return new ResponseContent();
    }
}