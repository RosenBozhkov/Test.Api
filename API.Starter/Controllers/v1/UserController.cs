using App.Metrics;
using Business.Interfaces.v1;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities.v1;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Test.Api.Controllers.v1;

/// <summary>
/// Controller for managing users
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class UserController : ControllerBase
{
    private static readonly string ControllerName = typeof(UserController).FullName!;
    private readonly IUserService _userService;
    private readonly ITelemetryProvider _telemetryProvider;
    private readonly IMetrics _metrics;

    /// <summary>
    /// Default constructor for controller
    /// </summary>
    /// <param name="modelService"></param>
    /// <param name="telemetryProvider"></param>
    /// <param name="metrics"></param>
    public UserController(
        IUserService modelService,
        ITelemetryProvider telemetryProvider,
        IMetrics metrics)
    {
        _telemetryProvider = telemetryProvider;
        _userService = modelService;
        _metrics = metrics;
    }

    /// <summary>
    /// Endpoint for creating a User
    /// </summary>
    /// <param name="request"></param>
    [Anonymous]
    [HttpPost("register")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<UserResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<UserResponse>> Register(UserRequest request)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        UserResponse registered = await _userService.RegisterAsync(request);

        return new ResponseContent<UserResponse>
        {
            Result = registered
        };
    }

    /// <summary>
    /// Endpoint for logging a user in
    /// </summary>
    /// <param name="request"></param>
    [Anonymous]
    [HttpPost("login")]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<UserResponse>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<string>> Login(UserRequest request)
    {
        //using var activity = _telemetryProvider.StartActivity(ControllerName, nameof(CreateAsync));
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DemoCounter);

        string registered = await _userService.LoginAsync(request);

        return new ResponseContent<string>
        {
            Result = registered
        };
    }
}
