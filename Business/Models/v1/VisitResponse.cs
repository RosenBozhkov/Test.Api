using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Visit response DTO
/// </summary>
public class VisitResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Date time of when the car is checked in the shop
    /// </summary>
    public DateTime CheckedIn { get ; set; }
    /// <summary>
    /// Date time of the delivery of the car. This means when the task is delivered to the owner fixed
    /// </summary>
    public DateTime? Completion { get; set; }
    /// <summary>
    /// Total price after all calculations
    /// </summary>
    public float TotalPrice { get; set; }

    /// <summary>
    /// Car for the Visit (nav prop)
    /// </summary>
    public CarResponse Car { get; set; }
    /// <summary>
    /// All Jobs of a Visit to the shop (nav prop)
    /// </summary>
    public ICollection<JobResponse> Jobs { get; set; } = new List<JobResponse>();
}
