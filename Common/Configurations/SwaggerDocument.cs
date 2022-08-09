using inacs.v8.nuget.DevAttributes;

namespace Common.Configurations;

/// <summary>
/// Swagger document information container
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class SwaggerDocument
{
    /// <summary>
    /// Name of the document
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Version of the document
    /// </summary>
    public string Version { get; set; } = string.Empty;
}