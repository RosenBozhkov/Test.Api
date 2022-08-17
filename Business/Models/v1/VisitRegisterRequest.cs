using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Visit request DTO for creating a visit
/// </summary>
public class VisitRegisterRequest
{
    /// <summary>
    /// Id of Car to be checked in
    /// </summary>
    public Guid CarId { get; set; }
    /// <summary>
    /// All Jobs of a Visit to the shop (nav prop)
    /// </summary>
    public List<int> JobIds { get; set; } = new List<int>();
}
