using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Test.Api.Models;
using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.ExposeDeveloper.Interfaces;
using inacs.v8.nuget.ExposeDeveloper.Models;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Test.Api.Controllers.MaybeCheckLater;

/// <summary>
/// Controller for exposing internal functionalities
/// </summary>
[HideFromSwagger]
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class InternalsController : ControllerBase
{
    private static readonly string ControllerName = typeof(InternalsController).FullName!;
    private readonly IExposeDeveloperService _exposeDeveloperService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly LoggingLevelSwitch _levelSwitch;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="exposeDeveloperService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="levelSwitch"></param>
    public InternalsController(IExposeDeveloperService exposeDeveloperService,
        ITelemetryProvider telemetryProvider,
        LoggingLevelSwitch levelSwitch)
    {
        _exposeDeveloperService = exposeDeveloperService;
        _telemetryProvider = telemetryProvider;
        _levelSwitch = levelSwitch;
    }

    /// <summary>
    /// Exposes Developer attribute metadata for types and methods
    /// </summary>
    /// <returns>All the Developer attribute values for types and methods</returns>
    [Authorization("INTERNAL")]
    [HttpGet("developers")]
    [Developer("Vasil Egov", "v.egov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<ExposedDevelopersInformation>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public ResponseContent<ExposedDevelopersInformation> GetDevelopers()
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetDevelopers));

        var result = _exposeDeveloperService.GetExposedDevelopers();

        return new ResponseContent<ExposedDevelopersInformation>
        {
            Result = result
        };
    }

    /// <summary>
    /// Gets all possible log levels
    /// </summary>
    /// <returns></returns>
    [Authorization("INTERNAL")]
    [HttpGet("logLevels")]
    [Developer("Vasil Egov", "v.egov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<LogLevelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public ResponseContent<LogLevelResponse> GetLogLevels()
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(GetLogLevels));

        return new ResponseContent<LogLevelResponse>
        {
            Result = new LogLevelResponse
            {
                Current = _levelSwitch.MinimumLevel.ToString(),
                Levels = Enum.GetNames<LogEventLevel>()
            }
        };
    }

    /// <summary>
    /// Changes current log levels
    /// </summary>
    /// <returns></returns>
    [Authorization("INTERNAL")]
    [HttpPost("logLevels")]
    [Developer("Vasil Egov", "v.egov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<LogLevelResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public ResponseContent<LogLevelResponse> ChangeLogLevel(LogLevelRequest logLevel)
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(ChangeLogLevel));

        _levelSwitch.MinimumLevel = Enum.Parse<LogEventLevel>(logLevel.LogLevel);

        return new ResponseContent<LogLevelResponse>
        {
            Result = new LogLevelResponse
            {
                Current = _levelSwitch.MinimumLevel.ToString(),
                Levels = Enum.GetNames<LogEventLevel>()
            }
        };
    }

    /// <summary>
    /// Gets all request headers and sends them in response
    /// </summary>
    /// <returns></returns>
    [Authorization("INTERNAL")]
    [HttpGet("echo")]
    [Developer("Vasil Egov", "v.egov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<IHeaderDictionary>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public ResponseContent<IHeaderDictionary> Echo()
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(Echo));

        return new ResponseContent<IHeaderDictionary>
        {
            Result = Request.Headers
        };
    }

    /// <summary>
    /// A redirect to swagger
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpGet("/help")]
    [Developer("Vasil Egov", "v.egov@itsoft.bg")]
    public ActionResult Help()
    {
        using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(Help));

        return Redirect("swagger");
    }
}