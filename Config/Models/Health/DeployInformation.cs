using inacs.v8.nuget.DevAttributes;

namespace Config.Models.Health;

/// <summary>
/// Deploy information for health check
/// </summary>
[Developer("Petar Dlagnekov", "p.dlagnekov@itsoft.bg")]
public class DeployInformation
{
    /// <summary>
    /// Deploy release id
    /// </summary>
    public string DeployRelease { get; set; } = string.Empty;
    /// <summary>
    /// Commit id for delploy
    /// </summary>
    public string DeployId { get; set; } = string.Empty;
    /// <summary>
    /// Instance of the container
    /// </summary>
    public string InstanceId { get; set; } = string.Empty;
    /// <summary>
    /// Deployment type or environment of the container
    /// </summary>
    public string DeploymentType { get; set; } = string.Empty;
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// Runtime SDK version
    /// </summary>
    public string RuntimeSDKVersion { get; set; } = string.Empty;
}