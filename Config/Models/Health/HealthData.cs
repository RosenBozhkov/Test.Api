using inacs.v8.nuget.DevAttributes;

namespace Config.Models.Health;

/// <summary>
/// Health Data
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class HealthData
{
    /// <summary>
    /// Allocated bytes
    /// </summary>
    public long AllocatedBytes { get; set; }
    /// <summary>
    /// Gen 0 collections
    /// </summary>
    public long Gen0Collections { get; set; }
    /// <summary>
    /// Gen 1 collections
    /// </summary>
    public long Gen1Collections { get; set; }
    /// <summary>
    /// Gen 2 collections
    /// </summary>
    public long Gen2Collections { get; set; }
}