using inacs.v8.nuget.DevAttributes;

namespace Config.Models.Health;

/// <summary>
/// Memory check options
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class MemoryCheckOptions
{
    /// <summary>
    /// Failure threshold (in bytes) 
    /// </summary>
    public long Threshold { get; set; } = 1024L * 1024L * 1024L;
}