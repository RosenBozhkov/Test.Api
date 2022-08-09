using inacs.v8.nuget.DevAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.v1;

/// <summary>
/// Visit entity
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
[Table("ApiStarter_Visits")]
public class Visit
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Date time of when the car is checked in the shop
    /// </summary>
    public DateTime CheckedIn { get; set; }
    /// <summary>
    /// Date time of the delivery of the car. This means that all the jobs are compleated
    /// </summary>
    public DateTime? Completion { get; set; }

    /// <summary>
    /// All Jobs of a Visit to the shop (nav prop)
    /// </summary>
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    /// <summary>
    /// Car for the Visit (nav prop)
    /// </summary>
    public Car Car { get; set; }
    /// <summary>
    /// Id of the Car for the Visit (nav prop)
    /// </summary>
    public Guid CarId { get; set; }
    /// <summary>
    /// Total price after all calculations
    /// </summary>
    public float TotalPrice { get; set; }
}