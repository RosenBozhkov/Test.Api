using Common.Resources;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Common.Configurations;
using inacs.v8.nuget.DevAttributes;

namespace Config.Swagger.Filters;

/// <summary>
/// Adds the version header on each operation and fills it with the version from the document.
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class AddVersionHeader : IOperationFilter
{
    private readonly ApplicationConfiguration _applicationConfiguration;

    /// <summary>
    /// Constructor with manual dependency provision
    /// </summary>
    /// <param name="applicationConfiguration"></param>
    public AddVersionHeader(ApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }
    /// <summary>
    /// Applies the filter for version header
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        string version = context.ApiDescription.GroupName ?? string.Empty;
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = _applicationConfiguration.VersionHeaderKey,
            Description = string.Format(Messages.VersionHeaderDescription, version),
            Schema = new OpenApiSchema
            {
                Example = new OpenApiString(version)
            },
            In = ParameterLocation.Header
        });
    }
}