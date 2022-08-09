using inacs.v8.nuget.DevAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.v1;

/// <summary>
/// Model entity
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
[Table("ApiStarter_Models")]
public class Model
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of Model
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Id of the Make for the Model (nav prop)
    /// </summary>
    public Guid MakeId { get; set; }
    /// <summary>
    /// Make for the Model (nav prop)
    /// </summary>
    public Make Make { get; set; }
    /// <summary>
    /// All Cars of a Model (nav prop)
    /// </summary>
    public ICollection<Car> Cars { get; set; } = new List<Car>();

}
