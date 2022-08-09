using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Config.Swagger.Filters;

/// <summary>
/// A swagger filter used to hide schemas marked with <see cref="HideFromSwaggerAttribute"/> from swagger
/// </summary>
public class ExcludeSchemaFilter : IDocumentFilter
{
    //Removes also response content containers. Swagger will generate a type CarResponseContent for a ResponseContent<Car>. This is used to remove them as well.
    private const string ResponseContentSuffix = nameof(ResponseContent);
    
    /// <summary>
    /// Hides schemas marked with <see cref="HideFromSwaggerAttribute"/> from swagger
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        List<string> typeNames = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetCustomAttribute<HideFromSwaggerAttribute>() is not null)
            .Select(t => t.Name)
            .ToList();
        
        //These types can not be attributed, so it is hardcoded.
        typeNames.Add(nameof(DeveloperInformation));
        typeNames.Add(nameof(IHeaderDictionary));
        
        foreach (var name in typeNames)
        {
            swaggerDoc.Components.Schemas.Remove(name);
            swaggerDoc.Components.Schemas.Remove(name + ResponseContentSuffix);
        }
    }
}