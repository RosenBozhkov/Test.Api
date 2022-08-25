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
using Microsoft.AspNetCore.Authorization;

namespace Test.Api.Controllers.v1;

/// <summary>
/// Controller for managing jobs
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class JobController : ControllerBase
{
    private static readonly string ControllerName = typeof(JobController).FullName!;
    private readonly IJobService _jobService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="jobService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public JobController(
        IJobService jobService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _jobService = jobService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for getting all the jobs
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<JobResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<IList<JobResponse>>> GetAllAsync()
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetAllAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        IList<JobResponse> jobs = await _jobService.GetAllAsync();

        return new ResponseContent<IList<JobResponse>>
        {
            Result = jobs
        };
    }

    /// <summary>
    /// Endpoint for getting a job by Id
    /// </summary>
    /// <param name="id"></param>
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Journal]
    [Anonymous]
    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<JobResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<JobResponse>> GetByIdAsync(int id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetByIdAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        JobResponse job = await _jobService.GetByIdAsync(id);

        return new ResponseContent<JobResponse>
        {
            Result = job
        };
    }

    /// <summary>
    /// Endpoint for creating Job
    /// </summary>
    /// <param name="job"></param>
    // [Authorization("ADMIN", "ROOT", "LEAF", "WOOD")]
    [Anonymous]
    [HttpPost]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<JobResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    // [HideRequestBody]
    // [HideResponseBody]
    public async Task<ResponseContent<JobResponse>> CreateAsync(JobCreateRequest job)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        JobResponse created = await _jobService.CreateAsync(job);

        return new ResponseContent<JobResponse>
        {
            Result = created
        };
    }

    /// <summary>
    /// Endpoint for updating a job by Id
    /// </summary>
    /// <param name="model"></param>
    [Anonymous]
    [HttpPut()]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<JobResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<JobResponse>> UpdateAsync(JobUpdateRequest model)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(UpdateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        JobResponse updated = await _jobService.UpdateAsync(model);

        return new ResponseContent<JobResponse>
        {
            Result = updated
        };
    }

    /// <summary>
    /// Endpoint for deleting a job by Id
    /// </summary>
    /// <param name="id"></param>
    [Anonymous]
    [Obsolete]
    [HttpDelete("{id:int}")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent> DeleteAsync(int id)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(DeleteAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        await _jobService.DeleteAsync(id);

        return new ResponseContent();
    }
}
