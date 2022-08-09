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
/// Controller for managing models
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class ModelController : ControllerBase
{
    private static readonly string ControllerName = typeof(ModelController).FullName!;
    private readonly IModelService _modelService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="modelService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public ModelController(
        IModelService modelService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _modelService = modelService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting all the models
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<ModelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IList<ModelResponse>>> GetAllAsync()
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetAllAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IList<ModelResponse> models = await _modelService.GetAllAsync();

        return new ResponseContent<IList<ModelResponse>>
        {
            Result = models
        };
    }

    /// <summary>
    /// Endpoint for getting a model by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Journal]
    [Anonymous]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<ModelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<ModelResponse>> GetByIdAsync(Guid id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetByIdAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        ModelResponse model = await _modelService.GetByIdAsync(id);

        return new ResponseContent<ModelResponse>
        {
            Result = model
        };
    }

    /// <summary>
    /// Endpoint for creating Model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    // [Authorization("ADMIN", "ROOT", "LEAF", "WOOD")]
    [Anonymous]
    [HttpPost]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<ModelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    // [HideRequestBody]
    // [HideResponseBody]
    public async Task<ResponseContent<ModelResponse>> CreateAsync(ModelRequest model)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        ModelResponse created = await _modelService.CreateAsync(model);

        return new ResponseContent<ModelResponse>
        {
            Result = created
        };
    }

    /// <summary>
    /// Endpoint for updating a model by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Anonymous]
    [HttpPut("{id:guid}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<ModelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<ModelResponse>> UpdateAsync(Guid id, ModelRequest model)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(UpdateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        ModelResponse updated = await _modelService.UpdateAsync(id, model);

        return new ResponseContent<ModelResponse>
        {
            Result = updated
        };
    }

    /// <summary>
    /// Endpoint for deleting a model by Id
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

        await _modelService.DeleteAsync(id);

        return new ResponseContent();
    }
}