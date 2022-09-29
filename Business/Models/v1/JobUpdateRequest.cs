using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Model for updating a Job
/// </summary>
public class JobUpdateRequest
{
    /// <summary>
    /// Job id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Basic price for a Job
    /// </summary>
    public int Price { get; set; }
}
