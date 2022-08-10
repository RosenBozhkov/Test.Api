using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.v1;

/// <summary>
/// Job repository interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IJobRepository
{
    /// <summary>
    /// Adds a job
    /// </summary>
    /// <param name="entity"></param>
    void Add(Job entity);
    /// <summary>
    /// Gets all jobs
    /// </summary>
    Task<IList<Job>> GetAllAsync();
    /// <summary>
    /// Get a job by Id
    /// </summary>
    /// <param name="id"></param>
    Task<Job?> GetByIdAsync(int id);
    /// <summary>
    /// Delete a job by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(int id);
    /// <summary>
    /// Delete a job
    /// </summary>
    /// <param name="entity"></param>
    void Delete(Job entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
    /// <summary>
    /// Validates that all ids are valid Job ids
    /// </summary>
    /// <param name="jobIds"></param>
    ICollection<Job> GetValidJobs(List<int> jobIds);
}
