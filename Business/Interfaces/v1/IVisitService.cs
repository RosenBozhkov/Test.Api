using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Business.Interfaces.v1;

/// <summary>
/// Visit interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IVisitService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    Task<VisitResponse> GetByIdAsync(Guid id);
    /// <summary>
    /// Get all
    /// </summary>
    Task<IList<VisitResponse>> GetAllAsync();
    /// <summary>
    /// Create visit
    /// </summary>
    /// <param name="model"></param>
    Task<VisitResponse> CreateAsync(VisitRegisterRequest model);
    /// <summary>
    /// Update visit
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    Task<VisitResponse> UpdateAsync(VisitFinishRequest model);
    /// <summary>
    /// Delete visit
    /// </summary>
    /// <param name="id"></param>
    Task DeleteAsync(Guid id);
}
