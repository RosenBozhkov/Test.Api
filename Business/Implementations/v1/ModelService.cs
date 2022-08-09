using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces.v1;
using Business.Models.v1;
using Common.Exceptions;
using Common.Resources;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.Core.Models;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Business.Validators.v1;

namespace Business.Implementations.v1;

/// <summary>
/// Model service
/// </summary>
public class ModelService : IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<ModelService> _logger;
    private readonly IMakeService _makeService;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="modelRepository"></param>
    /// <param name="validatorService"></param>
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public ModelService(IModelRepository modelRepository, IValidatorService validatorService, IMapper mapper,
        RequestState requestState, ILogger<ModelService> logger, IMakeService makeService)
    {
        _modelRepository = modelRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
        _makeService = makeService;
    }

    /// <summary>
    /// Get model by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<ModelResponse> GetByIdAsync(Guid id)
    {
        Model model = await _modelRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        return _mapper.Map<ModelResponse>(model);
    }

    /// <summary>
    /// Get all models
    /// </summary>
    /// <returns></returns>
    public async Task<IList<ModelResponse>> GetAllAsync()
    {
        var models = await _modelRepository.GetAllAsync();
        return _mapper.Map<IList<ModelResponse>>(models);
    }

    /// <summary>
    /// Create a model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ModelResponse> CreateAsync(ModelRequest model)
    {
        _validatorService.Validate(model);

        //validateMakeNameIsUnique is included in the following line!
        var make = await _makeService.CreateIfNotExist(model.MakeName);

        Model modelToCreate = new() { Name = model.Name, Make = make};
        _modelRepository.Add(modelToCreate);
        await _modelRepository.SaveChangesAsync();

        return _mapper.Map<ModelResponse>(modelToCreate);
    }

    /// <summary>
    /// Update a model
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ModelResponse> UpdateAsync(Guid id, ModelRequest model)
    {
        _validatorService.Validate(model);

        Model modelToUpdate = await _modelRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(Messages.ResourceNotFound);

        await _modelRepository.SaveChangesAsync();

        return _mapper.Map<ModelResponse>(modelToUpdate);
    }

    /// <summary>
    /// Delete a model by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        await _modelRepository.DeleteByIdAsync(id);
        await _modelRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Get a Model and Make by Name, if they do not exist, create them
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="makeName"></param>
    public async Task<Model> CreateIfNotExist(string modelName, string makeName)
    {
        var make = await _makeService.CreateIfNotExist(makeName);

        var model = await _modelRepository.GetOrCreateAsync(modelName, make);

        return model;
    }
}
