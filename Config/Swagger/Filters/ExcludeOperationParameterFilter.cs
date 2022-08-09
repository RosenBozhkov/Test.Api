using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using inacs.v8.nuget.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Config.Swagger.Filters;

/// <summary>
/// Removes the annotated with <see cref="HideFromSwaggerAttribute"/> parameters of an endpoint from swagger.
/// This filter hides the Principal parameter as well!
/// </summary>
public class ExcludeOperationParameterFilter : IOperationFilter
{
    /// <summary>
    /// Hidden properties are cached, as the call to get them is resource heavy, and thee operation filter is called multiple times
    /// for one swagger index request. So we cache it and make it only once per swagger index request
    /// </summary>
    private readonly List<string> _hiddenProperties;
    
    /// <summary>
    /// Constructor for filter
    /// </summary>
    public ExcludeOperationParameterFilter()
    {
        _hiddenProperties = GetHiddenParameterPropertyNames();
    }
    
    /// <summary>
    /// Hides operation parameters marked with <see cref="HideFromSwaggerAttribute"/>
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        List<string> hiddenTypes = GetHiddenParameterTypeNames(context);

        operation.Parameters ??= new List<OpenApiParameter>();

        foreach (var name in hiddenTypes)
        {
            OpenApiParameter? openApiParameter = operation.Parameters.FirstOrDefault(p => p.Name == name);
            operation.Parameters.Remove(openApiParameter);
        }

        foreach (var name in _hiddenProperties)
        {
            OpenApiParameter? openApiParameter = operation.Parameters.FirstOrDefault(p => p.Name == name);
            operation.Parameters.Remove(openApiParameter);
        }
    }

    /// <summary>
    /// Gets properties from types, that are used as a container for request parameters(query parameters, headers and so on),
    /// and are marked with <see cref="HideFromSwaggerAttribute"/>
    /// After the end of this filter, these properties will be removed from the parameters
    /// </summary>
    /// <returns></returns>
    private static List<string> GetHiddenParameterPropertyNames()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().SelectMany(t => t.GetProperties()))
            .Where(p => p.GetCustomAttribute<HideFromSwaggerAttribute>() is not null)
            .Select(p => p.Name)
            .ToList();
    }

    /// <summary>
    /// Gets types, that are used as a container for request parameters(query parameters, headers and so on), and are marked with <see cref="HideFromSwaggerAttribute"/>
    /// After the end of this filter, these types and all their fields will be removed from the parameters
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static List<string> GetHiddenParameterTypeNames(OperationFilterContext context)
    {
        return context.ApiDescription.ParameterDescriptions
            .Where(p => p.Type.GetCustomAttribute<HideFromSwaggerAttribute>() is not null)
            .Select(p => p.Name)
            .ToList();
    }
}