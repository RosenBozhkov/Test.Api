using inacs.v8.nuget.DevAttributes;
using System;

namespace Business.Models.v1;

/// <summary>
/// Car update owner and modifier request DTO
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarUpdateRequest
{
    /// <summary>
    /// Id of the car
    /// </summary>
    public Guid Id;
    /// <summary>
    /// A price multiple is a ratio that the company uses for each car individually
    /// </summary>
    public float Modifier { get; set; }
    /// <summary>
    /// Car owner
    /// </summary>
    public Guid UserId { get; set; }
}