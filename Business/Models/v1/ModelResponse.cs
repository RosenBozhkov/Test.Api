using inacs.v8.nuget.DevAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

/// <summary>
/// Model response
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class ModelResponse
{
    /// <summary>
    /// Model id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of Model
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Name of Make
    /// </summary>
    public string MakeName { get; set; } = string.Empty;
}
