using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Config.Models.Health;

/// <summary>
/// Health memory object
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class HealthMemory
{
    /// <summary>
    /// Constructor
    /// </summary>
    public HealthMemory()
    {
        Data = new HealthData();
    }

    /// <summary>
    /// Status
    /// </summary>
    public HealthStatus Status { get; set; }
    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Data
    /// </summary>
    public HealthData Data { get; set; }
}