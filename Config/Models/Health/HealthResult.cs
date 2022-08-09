
using inacs.v8.nuget.DevAttributes;

namespace Config.Models.Health;

/// <summary>
/// Health result
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class HealthResult
{
    /// <summary>
    /// Constructor
    /// </summary>
    public HealthResult()
    {
        Memory = new HealthMemory();
    }
        
    /// <summary>
    /// Health memory
    /// </summary>
    public HealthMemory Memory { get; set; }
}