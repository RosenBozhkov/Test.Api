using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.v1;

/// <summary>
/// Visit repository interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IVisitRepository
{
    /// <summary>
    /// Adds a visit
    /// </summary>
    /// <param name="entity"></param>
    void Add(Visit entity);
    /// <summary>
    /// Gets all visits
    /// </summary>
    Task<IList<Visit>> GetAllAsync();
    /// <summary>
    /// Get a visit by Id
    /// </summary>
    /// <param name="id"></param>
    Task<Visit?> GetByIdAsync(Guid id);
    /// <summary>
    /// Delete a visit by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(Guid id);
    /// <summary>
    /// Delete a visit
    /// </summary>
    /// <param name="entity"></param>
    void Delete(Visit entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
}
