using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using inacs.v8.nuget.DevAttributes;

namespace Config.Swagger.Filters;

/// <summary>
/// Swagger filter that updates enumerations in swagger
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Applies the filter
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(n => schema.Enum.Add(new OpenApiString($"{n}: {(int)Enum.Parse(context.Type, n)}")));
        }
    }
}