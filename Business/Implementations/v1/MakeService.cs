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
using System.Linq;

namespace Business.Implementations.v1;

/// <summary>
/// Make service
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class MakeService : IMakeService
{
    private readonly IMakeRepository _makeRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<MakeService> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="makeRepository"></param>
    /// <param name="validatorService"></param>
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public MakeService(IMakeRepository makeRepository, IValidatorService validatorService, IMapper mapper,
        RequestState requestState, ILogger<MakeService> logger)
    {
        _makeRepository = makeRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
    }

    /// <summary>
    /// Get make by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<MakeResponse> GetByIdAsync(Guid id)
    {
        Make make = await _makeRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        //int allCarsOfMake = make.Models.Select(m => m.Cars.Count).Sum();
        
        var result = _mapper.Map<MakeResponse>(make);
        //result.Count = allCarsOfMake;
        return result;
    }

    /// <summary>
    /// Get all makes
    /// </summary>
    /// <returns></returns>
    public async Task<IList<MakeResponse>> GetAllAsync()
    {
        var makes = await _makeRepository.GetAllAsync();

        var result = _mapper.Map<IList<MakeResponse>>(makes);

        //foreach (var i in makes)
        //{
        //    int allCarsOfMake = i.Models.Select(m => m.Cars.Count).Sum();
        //
        //    result.First(m => m.Name == i.Name).Count = allCarsOfMake;
        //}

        return result;
    }

    /// <summary>
    /// Create a make
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<MakeResponse> CreateAsync(MakeRequest model)
    {
        _validatorService.Validate(model);

        Make make = new() { Name = model.Name};
        _makeRepository.Add(make);
        await _makeRepository.SaveChangesAsync();

        return _mapper.Map<MakeResponse>(make);
    }

    /// <summary>
    /// Update a make
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<MakeResponse> UpdateAsync(Guid id, MakeRequest model)
    {
        _validatorService.Validate(model);

        Make make = await _makeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(Messages.ResourceNotFound);

        await _makeRepository.SaveChangesAsync();

        return _mapper.Map<MakeResponse>(make);
    }

    /// <summary>
    /// Delete a make by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        await _makeRepository.DeleteByIdAsync(id);
        await _makeRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Get a make by Name, if it does not exist, create one
    /// </summary>
    /// <param name="name"></param>
    public async Task<Make> CreateIfNotExist(string name)
    {
        var make = await _makeRepository.GetOrCreateAsync(name);

        return make;
    }
}