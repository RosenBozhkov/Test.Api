using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using inacs.v8.nuget.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Config.Swagger.Filters;

/// <summary>
/// A swagger filter used to hide properties marked with <see cref="HideFromSwaggerAttribute"/> from a schema
/// </summary>
public class ExcludeFieldFromSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Hides the properties marked with <see cref="HideFromSwaggerAttribute"/>
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        IEnumerable<PropertyInfo> propertyToExclude = context
            .Type.GetProperties().Where(p => p.GetCustomAttribute<HideFromSwaggerAttribute>() is not null);

        foreach (PropertyInfo property in propertyToExclude)
        {   
            schema.Properties.Remove(property.Name);
            string camelCaseName = char.ToLower(property.Name[0]) + property.Name[1..];
            schema.Properties.Remove(camelCaseName);
        }
    }
}