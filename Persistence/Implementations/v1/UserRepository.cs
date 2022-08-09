using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

/// <summary>
/// User repository
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ThingsContext _context;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="context"></param>
    public UserRepository(ThingsContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a user
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Add(User entity)
    {
        _context.Users.Add(entity);
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns></returns>
    public async Task<IList<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// Get a user by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> GetByIdAsync(Guid id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    /// <summary>
    /// Get a user by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<User?> GetByNameAsync(string name)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == name);

        return user;
    }

    /// <summary>
    /// Delete a user by Id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(Guid id)
    {
        User? entity = await GetByIdAsync(id);

        if (entity is not null)
        {
            Delete(entity);
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(User entity)
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
    /// Check if username is free, throws if it is not.
    /// </summary>
    /// <param name="name"></param>
    public async Task ValidateUsernameNotExist(string name)
    {
        if (await _context.Users.AnyAsync(u => u.Username == name))
        {
            throw new InvalidUsernameException("User with that name already exists.");
        }
    }
}
