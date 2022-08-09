using inacs.v8.nuget.DevAttributes;
using System;

namespace Test.Api.Models.View.v1;

/// <summary>
/// Some example model
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class Activities
{
    /// <summary>
    /// Example description
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Example description
    /// </summary>
    public int ActivitiesCount { get; set; }
}