using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;

namespace Test.Api.Models;

/// <summary>
/// Response containing the current log level and possible options
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
[HideFromSwagger]
public class LogLevelResponse
{
    /// <summary>
    /// Current log level of the service
    /// </summary>
    public string Current { get; set; } = string.Empty;

    /// <summary>
    /// All possible options
    /// </summary>
    public string[] Levels { get; set; } = System.Array.Empty<string>();
}