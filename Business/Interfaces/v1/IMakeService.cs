using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Business.Interfaces.v1;

/// <summary>
/// Make service interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IMakeService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MakeResponse> GetResponseByIdAsync(Guid id);
    /// <summary>
    /// Get all
    /// </summary>
    /// <returns></returns>
    Task<IList<MakeResponse>> GetAllAsync();
    /// <summary>
    /// Create make
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<MakeResponse> CreateAsync(MakeRequest model);
    /// <summary>
    /// Update make
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<MakeResponse> UpdateAsync(Guid id, MakeRequest model);
    /// <summary>
    /// Delete make
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// Get By Name, if it does not exist, create one
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Make> CreateIfNotExist(string name);
}