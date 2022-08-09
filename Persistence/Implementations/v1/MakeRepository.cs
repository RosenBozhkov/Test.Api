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
/// Make repository
/// </summary>
public class MakeRepository : IMakeRepository
{
    private readonly ThingsContext _context;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public MakeRepository(ThingsContext context)
    {
        _context = context;
    }

    private IQueryable<Make> MakesQuery
    {
        get => _context.Makes
            .Include(c => c.Models)
            .ThenInclude(m => m.Cars);
    }

    /// <summary>
    /// Adds a make
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(Make entity)
    {
        _context.Makes.Add(entity);
    }

    /// <summary>
    /// Gets all makes
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Make>> GetAllAsync()
    {
        return await MakesQuery.ToListAsync();
    }

    /// <summary>
    /// Get a make by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Make?> GetByIdAsync(Guid id)
    {
        return await MakesQuery.FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Get a make by Name, if it does not exist, create one
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<Make?> GetOrCreateAsync(string name)
    {
        var make = await _context.Makes.FirstOrDefaultAsync(m => m.Name == name);

        if (make is null)
        {
            Make makeToCreate = new() { Name = name };
            _context.Add(makeToCreate);
            await _context.SaveChangesAsync();

            make = await _context.Makes.FirstOrDefaultAsync(m => m.Name == name);
        }

        return make;
    }

    /// <summary>
    /// Delete a make by Id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(Guid id)
    {
        Make? entity = await GetByIdAsync(id);

        if (entity is not null)
        {
            Delete(entity);
        }
    }

    /// <summary>
    /// Delete a make
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Make entity)
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