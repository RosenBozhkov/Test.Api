using System;
using System.Collections.Generic;
using System.Linq;
using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Business.Models.v1;

/// <summary>
/// Car response
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarResponse
{
    /// <summary>
    /// Car id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Owner of the Car
    /// </summary> 
    public string UserName { get; set; }
    /// <summary>
    /// Car Make
    /// </summary>
    public string ModelMakeName { get; set; } = string.Empty;
    /// <summary>
    /// Car Model
    /// </summary>
    //[HideFromSwagger]
    public string ModelName { get; set; } = string.Empty;
    /// <summary>
    /// Car year of creation
    /// </summary>
    public int YearOfCreation { get; set; }
    /// <summary>
    /// A price multiple is a ratio that the company uses for each car individually
    /// </summary>
    public float Modifier { get; set; }
}