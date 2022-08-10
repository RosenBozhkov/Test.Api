using inacs.v8.nuget.DevAttributes;
using System;

namespace Business.Models.v1;

/// <summary>
/// Car request DTO
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarCreateRequest
{
    /// <summary>
    /// Car Year of creation
    /// </summary>
    public int YearOfCreation { get; set; }
    /// <summary>
    /// Car Model
    /// </summary>
    public string ModelName { get; set; } = string.Empty;
    /// <summary>
    /// Car Make
    /// </summary>
    public string MakeName { get; set; } = string.Empty;
    /// <summary>
    /// A price multiple is a ratio that the company uses for each car individually
    /// </summary>
    public float Modifier { get; set; }
    /// <summary>
    /// Car owner
    /// </summary>
    public Guid UserId { get; set; }
}