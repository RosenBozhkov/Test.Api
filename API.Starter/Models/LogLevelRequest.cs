using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;

namespace Test.Api.Models;

/// <summary>
/// Request to change the log level of the application
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
[HideFromSwagger]
public class LogLevelRequest
{
    /// <summary>
    /// New log level
    /// </summary>
    public string LogLevel { get; set; } = string.Empty;
}