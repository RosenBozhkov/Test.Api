using System;
using System.ComponentModel.DataAnnotations.Schema;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.Abstract;

namespace Persistence.Entities.v1;

/// <summary>
/// Car entity
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
[Table("ApiStarter_Cars")]
public class Car : BaseEntity
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Year of creation
    /// </summary>
    public int YearOfCreation { get; set; }
    /// <summary>
    /// A price multiple is a ratio that the company uses for each car individually
    /// </summary>
    public float Modifier { get; set; }

    /// <summary>
    /// Id of the Model for the Car (nav prop)
    /// </summary>
    public Guid ModelId { get; set; }
    /// <summary>
    /// Model (nav prop)
    /// </summary>
    public Model Model { get; set; }
    /// <summary>
    /// Id of the User for the Car (nav prop)
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// User (nav prop)
    /// </summary>
    public User User { get; set; }
}