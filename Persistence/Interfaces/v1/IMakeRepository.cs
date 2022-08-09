using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.v1;

/// <summary>
/// Make repository interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IMakeRepository
{
    /// <summary>
    /// Adds a make
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Add(Make entity);
    /// <summary>
    /// Gets all makes
    /// </summary>
    /// <returns></returns>
    Task<IList<Make>> GetAllAsync();
    /// <summary>
    /// Get a make by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Make?> GetByIdAsync(Guid id);
    /// <summary>
    /// Get a make by Name
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Make?> GetOrCreateAsync(string name);
    /// <summary>
    /// Delete a make by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(Guid id);
    /// <summary>
    /// Delete a make
    /// </summary>
    /// <param name="entity"></param>
    void Delete(Make entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
}
