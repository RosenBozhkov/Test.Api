using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.v1;

/// <summary>
/// Job entity
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
[Table("ApiStarter_Jobs")]
public class Job : BaseEntity
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the Job
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Basic price for the Job
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// All visits that include the Job
    /// </summary>
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
