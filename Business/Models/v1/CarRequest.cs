using inacs.v8.nuget.DevAttributes;

namespace Business.Models.v1;

/// <summary>
/// Car request DTO
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarRequest
{
    /// <summary>
    /// Car Year of creation
    /// </summary>
    public int YearOfCreation { get; set; }
    /// <summary>
    /// Car Model
    /// </summary>
    //[HideFromSwagger]
    public string ModelName { get; set; } = string.Empty;
    /// <summary>
    /// Car Make
    /// </summary>
    public string MakeName { get; set; } = string.Empty;
    /// <summary>
    /// A price multiple is a ratio that the company uses for each car individually
    /// </summary>
    public float Modifier { get; set; }
}