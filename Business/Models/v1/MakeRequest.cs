using inacs.v8.nuget.DevAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Make request
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class MakeRequest
{
    /// <summary>
    /// Name of Make
    /// </summary>
    public string Name { get; set; }
}
