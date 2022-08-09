using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

public class JobUpdateRequest
{
    /// <summary>
    /// Basic price for a Job
    /// </summary>
    public int Price { get; set; }
}
