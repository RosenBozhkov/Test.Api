using System;
using Config.Filters.Action;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Config.Filters;

/// <summary>
/// Attribute marking hiding the request body when logged in <see cref="LogRequestFilter"/>
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class HideRequestBodyAttribute : Attribute, IFilterMetadata { }