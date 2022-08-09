using System.Linq;
using System.Reflection;
using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.EnvHelper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Config.Swagger.Filters;

/// <summary>
/// Filter for hiding actions and controllers from swagger. Hides from all environments, except local
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class HideActionFilter : IDocumentFilter
{
    /// <summary>
    /// Hides the routes from swagger
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (EnvironmentHelper.IsLocal)
        {
            return;
        }
            
        foreach (var contextApiDescription in context.ApiDescriptions)
        {
            var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

            if (!actionDescriptor.ControllerTypeInfo.GetCustomAttributes<HideFromSwaggerAttribute>().Any() &&
                !actionDescriptor.MethodInfo.GetCustomAttributes<HideFromSwaggerAttribute>().Any())
            {
                continue;
            }

            var key = "/" + contextApiDescription.RelativePath!.TrimEnd('/');
            swaggerDoc.Paths.Remove(key);
        }
    }
}