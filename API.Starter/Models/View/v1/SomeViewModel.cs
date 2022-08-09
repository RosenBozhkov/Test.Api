using inacs.v8.nuget.DevAttributes;

namespace Test.Api.Models.View.v1;

/// <summary>
/// Example description
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class SomeViewModel
{
    /// <summary>
    /// Example description
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Example description
    /// </summary>
    public decimal Something { get; set; }
}