using inacs.v8.nuget.Core.Attributes;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Make response
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class MakeResponse
{
    /// <summary>
    /// Make id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of Make
    /// </summary>
    public string Name { get; set; }

    [HideFromSwagger]
    public ICollection<Model> Models { get; set; } = new List<Model>();
    public int Count 
    {
        get => this.Models.Select(m => m.Cars.Count).Sum();
        //set;
    }
}
