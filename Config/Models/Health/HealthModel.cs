using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Config.Models.Health;

/// <summary>
/// Model for health checks
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class HealthModel
{
    /// <summary>
    /// Base constructor for health checks
    /// </summary>
    public HealthModel()
    {
        DeployInformation = new DeployInformation();
        Results = new HealthResult();
    }

    /// <summary>
    /// Deployment information for service
    /// </summary>
    public DeployInformation DeployInformation { get; set; }
    /// <summary>
    /// Status of the service in regards to memory
    /// </summary>
    public HealthStatus Status { get; set; }
    /// <summary>
    /// Health results
    /// </summary>
    public HealthResult Results { get; set; }
    /// <summary>
    /// Threads used for the current service
    /// </summary>
    public int ThreadsCount { get; set; }
}