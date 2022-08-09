using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Last Visit request DTO
/// </summary>
public class VisitFinishRequest
{
    /// <summary>
    /// Final points of damage
    /// </summary>
    public int AdditionalCost { get; set; }
}
