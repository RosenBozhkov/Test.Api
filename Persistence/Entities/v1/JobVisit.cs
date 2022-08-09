using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.v1;

/// <summary>
/// Intermediate table
/// </summary>
public class JobVisit
{
    public Guid VisitId { get; set; }
    public Visit Visit { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
}
