using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

/// <summary>
/// Model repository
/// </summary>
public class ModelRepository : IModelRepository
{
    private readonly ThingsContext _context;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public ModelRepository(ThingsContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a model
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(Model entity)
    {
        _context.Models.Add(entity);
    }

    /// <summary>
    /// Gets all models
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Model>> GetAllAsync()
    {
        return await _context.Models.ToListAsync();
    }

    /// <summary>
    /// Get a model by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Model?> GetByIdAsync(Guid id)
    {
        return await _context.Models.FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Get a Model by Name, if it does not exist, create one
    /// </summary>
    /// <param name="name"></param>
    /// <param name="make"></param>
    /// <returns></returns>
    public async Task<Model?> GetOrCreateAsync(string name, Make make)
    {
        var model = await _context.Models.FirstOrDefaultAsync(m => m.Name == name);

        if (model is null)
        {
            Model modelToCreate = new() { Name = name, Make = make };
            _context.Add(modelToCreate);
            await _context.SaveChangesAsync();

            model = await _context.Models.FirstOrDefaultAsync(m => m.Name == name);
        }

        return model;
    }

    /// <summary>
    /// Delete a model by Id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(Guid id)
    {
        Model? entity = await GetByIdAsync(id);

        if (entity is not null)
        {
            Delete(entity);
        }
    }

    /// <summary>
    /// Delete a model
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Model entity)
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