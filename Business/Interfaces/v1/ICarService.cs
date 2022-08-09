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
    /// <returns></returns>
    Task<CarResponse> GetByIdAsync(Guid id);
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Car> GetCarByIdAsync(Guid id);
    /// <summary>
    /// Get all
    /// </summary>
    /// <returns></returns>
    Task<IList<CarResponse>> GetAllAsync();
    /// <summary>
    /// Create car
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<CarResponse> CreateAsync(CarRequest model);
    /// <summary>
    /// Update car
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<CarResponse> UpdateAsync(Guid id, CarRequest model);
    /// <summary>
    /// Delete Car
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
}