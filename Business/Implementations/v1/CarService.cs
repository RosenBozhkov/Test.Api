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
/// Car service
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<CarService> _logger;
    private readonly IModelService _modelService;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="carRepository"></param>
    /// <param name="validatorService"></param>
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    /// <param name="modelService"></param>
    public CarService(ICarRepository carRepository, IValidatorService validatorService, IMapper mapper,
        RequestState requestState, ILogger<CarService> logger, IModelService modelService)
    {
        _carRepository = carRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
        _modelService = modelService;
    }

    /// <summary>
    /// Get car by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> CarResponse </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<CarResponse> GetByIdAsync(Guid id)
    {
        Car car = await _carRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        return _mapper.Map<CarResponse>(car);
    }
    
    /// <summary>
    /// Get car by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Car </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<Car> GetCarByIdAsync(Guid id)
    {
        Car car = await _carRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        return car;
    }
        
    /// <summary>
    /// Get all cars
    /// </summary>
    /// <returns></returns>
    public async Task<IList<CarResponse>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return _mapper.Map<IList<CarResponse>>(cars);
    }

    /// <summary>
    /// Create a car
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<CarResponse> CreateAsync(CarRequest model)
    {
        _validatorService.Validate(model);

        var theModel = await _modelService.CreateIfNotExist(model.ModelName, model.MakeName);

        Car car = new() { YearOfCreation = model.YearOfCreation, Model = theModel, Modifier = model.Modifier };

        _carRepository.Add(car);
        await _carRepository.SaveChangesAsync();

        return _mapper.Map<CarResponse>(car);
    }

    /// <summary>
    /// Update a car
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<CarResponse> UpdateAsync(Guid id, CarRequest model)
    {
        _validatorService.Validate(model);

        Car car = await _carRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(Messages.ResourceNotFound);

        car.YearOfCreation = model.YearOfCreation;
        await _carRepository.SaveChangesAsync();

        return _mapper.Map<CarResponse>(car);
    }

    /// <summary>
    /// Delete a car by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        await _carRepository.DeleteByIdAsync(id);
        await _carRepository.SaveChangesAsync();
    }
}