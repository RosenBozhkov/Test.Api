using inacs.v8.nuget.DevAttributes;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Config.Filters;

/// <summary>
/// Filter used as an attribute to mark a method for journal logging
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class JournalAttribute : Attribute, IFilterMetadata { }