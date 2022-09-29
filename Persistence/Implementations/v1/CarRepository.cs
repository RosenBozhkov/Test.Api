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
/// Car repository
/// </summary>
public class CarRepository : ICarRepository
{
    private readonly ThingsContext _context;
        
    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public CarRepository(ThingsContext context)
    {
        _context = context;
    }

    private IQueryable<Car> CarsQuery
    {
        get => _context.Cars
            .Include(c => c.Model)
            .ThenInclude(m => m.Make);
    }

    /// <summary>
    /// Adds a car
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(Car entity)
    {
        _context.Cars.Add(entity);
    }

    /// <summary>
    /// Gets all cars
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Car>> GetAllAsync()
    {
        return await CarsQuery.ToListAsync();
    }

    /// <summary>
    /// Get a car by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Car?> GetByIdAsync(Guid id)
    {
        return await CarsQuery.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Delete a car
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Car entity)
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