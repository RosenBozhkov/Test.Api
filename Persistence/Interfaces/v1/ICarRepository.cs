using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

/// <summary>
/// Car repository interface
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public interface ICarRepository
{
    /// <summary>
    /// Adds a car
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Add(Car entity);
    /// <summary>
    /// Gets all cars
    /// </summary>
    /// <returns></returns>
    Task<IList<Car>> GetAllAsync();
    /// <summary>
    /// Get a car by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Car?> GetByIdAsync(Guid id);
    /// <summary>
    /// Delete a car by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(Guid id);
    /// <summary>
    /// Delete a car
    /// </summary>
    /// <param name="entity"></param>
    void Delete(Car entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
}