using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

/// <summary>
/// Visit repository
/// </summary>
public class VisitRepository : IVisitRepository
{
    private readonly ThingsContext _context;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public VisitRepository(ThingsContext context)
    {
        _context = context;
    }

    private IQueryable<Visit> VisitsQuery
    {
        get => _context.Visits.Include(v => v.Jobs).Include(v => v.Car).ThenInclude(c => c.Model).ThenInclude(m => m.Make);
    }

    /// <summary>
    /// Adds a visit
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(Visit entity)
    {
        _context.Visits.Add(entity);
    }

    /// <summary>
    /// Gets all visits
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Visit>> GetAllAsync()
    {
        return await VisitsQuery.ToListAsync();
    }

    /// <summary>
    /// Get a visit by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Visit?> GetByIdAsync(Guid id)
    {
        return await VisitsQuery.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Delete a visit by Id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(Guid id)
    {
        Visit? entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            Delete(entity);
        }
    }

    /// <summary>
    /// Delete a visit
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Visit entity)
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
}
