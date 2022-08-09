using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.v1;

/// <summary>
/// User repository interface
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a user
    /// </summary>
    /// <param name="entity"></param>
    void Add(User entity);
    /// <summary>
    /// Gets all user
    /// </summary>
    Task<IList<User>> GetAllAsync();
    /// <summary>
    /// Get a user by name
    /// </summary>
    /// <param name="name"></param>
    Task<User?> GetByNameAsync(string name);
    /// <summary>
    /// Get a user by Id
    /// </summary>
    /// <param name="id"></param>
    Task<User?> GetByIdAsync(Guid id);
    /// <summary>
    /// Delete a user by Id
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(Guid id);
    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="entity"></param>
    void Delete(User entity);
    /// <summary>
    /// Save changes
    /// </summary>
    Task SaveChangesAsync();
    /// <summary>
    /// Check if username is free, throws if it is not.
    /// </summary>
    /// <param name="name"></param>
    Task ValidateUsernameNotExist(string name);
}