using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.v1;

/// <summary>
/// Model repository interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public  interface IModelRepository
{
    /// <summary>
    /// Adds a model
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Add(Model entity);
    /// <summary>
    /// Gets all models
    /// </summary>
    /// <returns></returns>
    Task<IList<Model>> GetAllAsync();
    /// <summary>
    /// Get a model by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model?> GetByIdAsync(Guid id);
    /// <summary>
    /// Delete a model by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(Guid id);
    /// <summary>
    /// Delete a model
    /// </summary>
    /// <param name="entity"></param>
    void Delete(Model entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
    /// <summary>
    /// Get a Model by Name, if it does not exist, create one
    /// </summary>
    /// <param name="name"></param>
    /// <param name="make"></param>
    /// <returns></returns>
    Task<Model?> GetOrCreateAsync(string name, Make make);
}
