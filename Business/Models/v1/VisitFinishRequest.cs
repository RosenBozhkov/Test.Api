using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Visit request DTO for finalizing the visit
/// </summary>
public class VisitFinishRequest
{
    /// <summary>
    /// Id of the visit
    /// </summary>
    public Guid Id {  get; set; }
    /// <summary>
    /// Final points of damage
    /// </summary>
    public int AdditionalCost { get; set; }
}
