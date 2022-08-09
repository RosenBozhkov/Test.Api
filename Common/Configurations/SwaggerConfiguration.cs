using System.Collections.Generic;
using inacs.v8.nuget.DevAttributes;

namespace Common.Configurations;

/// <summary>
/// Swagger documents information
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class SwaggerConfiguration
{
    /// <summary>
    /// List of all swagger documents
    /// </summary>
    public IList<SwaggerDocument> Documents { get; set; } = new List<SwaggerDocument>();
}