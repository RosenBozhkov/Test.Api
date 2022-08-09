using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Initial Visit request DTO
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
    public ICollection<int> JobIds { get; set; } = new List<int>();
}
