using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

/// <summary>
/// Job repository
/// </summary>
public class JobRepository : IJobRepository
{
    private readonly ThingsContext _context;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public JobRepository(ThingsContext context)
    {
        _context = context;
    }

    private IQueryable<Job> JobsQuery
    {
        get => _context.Jobs/*.Include(v => v.Job)*/;
    }

    /// <summary>
    /// Adds a job
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(Job entity)
    {
        _context.Jobs.Add(entity);
    }

    /// <summary>
    /// Gets all jobs
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Job>> GetAllAsync()
    {
        return await JobsQuery.ToListAsync();
    }

    /// <summary>
    /// Get a job by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Job?> GetByIdAsync(int id)
    {
        return await JobsQuery.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Delete a job by Id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(int id)
    {
        Job? entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            Delete(entity);
        }
    }

    /// <summary>
    /// Delete a job
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Job entity)
    {
        _context.Remove(entity);
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Validates that all ids are valid Job ids
    /// </summary>
    /// <param name="jobIds"></param>
    public ICollection<Job> GetValidJobs(List<int> jobIds)
    {
        List<Job> jobList = new();

        foreach (var jobId in jobIds)
        {
            var job = this.JobsQuery.FirstOrDefault(o => o.Id == jobId);

            if (job is null)
            {
                throw new NotFoundException("Job does not exist");
            }

            jobList.Add(job);
        }

        return jobList;
    }
}