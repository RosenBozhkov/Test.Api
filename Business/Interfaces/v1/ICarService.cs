using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Business.Interfaces.v1;

/// <summary>
/// Car service Interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface ICarService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    Task<CarResponse> GetResponseByIdAsync(Guid id);
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    Task<Car> GetByIdAsync(Guid id);
    /// <summary>
    /// Get all
    /// </summary>
    Task<IList<CarResponse>> GetAllAsync();
    /// <summary>
    /// Create car
    /// </summary>
    /// <param name="model"></param>
    Task<CarResponse> CreateAsync(CarCreateRequest model);
    /// <summary>
    /// Update car
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    Task<CarResponse> UpdateAsync(CarUpdateRequest model);
    /// <summary>
    /// Delete Car
    /// </summary>
    /// <param name="id"></param>
    Task DeleteAsync(Guid id);
}