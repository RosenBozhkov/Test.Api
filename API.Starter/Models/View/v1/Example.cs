using inacs.v8.nuget.DevAttributes;
using System;

namespace Test.Api.Models.View.v1;

/// <summary>
/// Example description
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class Example
{
    /// <summary>
    /// Example description
    /// </summary>
    public Guid SomeGuid { get; set; }
}