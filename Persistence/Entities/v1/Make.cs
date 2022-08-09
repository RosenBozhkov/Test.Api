using inacs.v8.nuget.DevAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.v1;

/// <summary>
/// Make entity
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
[Table("ApiStarter_Makes")]
public class Make
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of Make
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// All Models of a Make (nav prop)
    /// </summary>
    public ICollection<Model> Models { get; set; } = new List<Model>();
}
