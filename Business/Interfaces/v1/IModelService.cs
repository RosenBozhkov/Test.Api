using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.v1;

/// <summary>
/// Model Service Interface
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public interface IModelService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ModelResponse> GetResponseByIdAsync(Guid id);
    /// <summary>
    /// Get all
    /// </summary>
    /// <returns></returns>
    Task<IList<ModelResponse>> GetAllAsync();
    /// <summary>
    /// Create model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ModelResponse> CreateAsync(ModelRequest model);
    /// <summary>
    /// Update model
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ModelResponse> UpdateAsync(Guid id, ModelRequest model);
    /// <summary>
    /// Delete Model
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// Get a Model and Make by Name, if they do not exist, create them
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="makeName"></param>
    /// <returns></returns>
    Task<Model> CreateIfNotExist(string modelName, string makeName);
}
