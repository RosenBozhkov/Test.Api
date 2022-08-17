using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Business.Interfaces.v1;

/// <summary>
/// Job service Interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IJobService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    Task<JobResponse> GetByIdAsync(int id);
    /// <summary>
    /// Get all
    /// </summary>
    Task<IList<JobResponse>> GetAllAsync();
    /// <summary>
    /// Create job
    /// </summary>
    /// <param name="model"></param>
    Task<JobResponse> CreateAsync(JobCreateRequest model);
    /// <summary>
    /// Update job
    /// </summary>
    /// <param name="model"></param>
    Task<JobResponse> UpdateAsync(JobUpdateRequest model);
    /// <summary>
    /// Delete job
    /// </summary>
    /// <param name="id"></param>
    Task DeleteAsync(int id);
}
