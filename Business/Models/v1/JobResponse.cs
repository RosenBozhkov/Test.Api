using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Job response DTO
/// </summary>
public class JobResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of Model
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Basic price for a Job
    /// </summary>
    public int Price { get; set; }
}
