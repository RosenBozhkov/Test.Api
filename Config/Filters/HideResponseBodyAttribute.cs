using System;
using Config.Filters.Result;
using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Config.Filters;

/// <summary>
/// Attribute marking hiding the request body when logged in <see cref="LogResponseFilter"/>
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class HideResponseBodyAttribute : Attribute, IFilterMetadata { }