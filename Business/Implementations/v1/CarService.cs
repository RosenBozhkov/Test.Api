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
    private readonly IUserService _userService;

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
        RequestState requestState, ILogger<CarService> logger, IModelService modelService, IUserService userService)
    {
        _carRepository = carRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
        _modelService = modelService;
        _userService = userService;
    }

    /// <summary>
    /// Get car by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> CarResponse </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<CarResponse> GetResponseByIdAsync(Guid id)
    {
        Car car = await GetByIdAsync(id);

        return _mapper.Map<CarResponse>(car);
    }
    
    /// <summary>
    /// Get car by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Car </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<Car> GetByIdAsync(Guid id)
    {
        Car car = await _carRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        return car;
    }
        
    /// <summary>
    /// Get all cars
    /// </summary>
    public async Task<IList<CarResponse>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return _mapper.Map<IList<CarResponse>>(cars);
    }

    /// <summary>
    /// Create a car
    /// </summary>
    /// <param name="model"></param>
    public async Task<CarResponse> CreateAsync(CarCreateRequest model)
    {
        _validatorService.Validate(model);

        User owner = await _userService.GetUserByIdAsync(model.UserId);

        Model carModel = await _modelService.CreateIfNotExist(model.ModelName, model.MakeName);

        Car car = new() { YearOfCreation = model.YearOfCreation, Model = carModel, Modifier = model.Modifier, User = owner };

        _carRepository.Add(car);
        await _carRepository.SaveChangesAsync();

        return _mapper.Map<CarResponse>(car);
    }

    /// <summary>
    /// Update the Car's owner and repairs cost modifier
    /// </summary>
    /// <param name="model"></param>
    public async Task<CarResponse> UpdateAsync(CarUpdateRequest model)
    {
        _validatorService.Validate(model);

        Car car = await GetByIdAsync(model.Id);

        if (model.UserId != Guid.Empty)
        {
            User newOwner = await _userService.GetUserByIdAsync(model.UserId);
            car.User = newOwner;
        }
        
        car.Modifier = model.Modifier;
        await _carRepository.SaveChangesAsync();

        return _mapper.Map<CarResponse>(car);
    }

    /// <summary>
    /// Delete a car by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        Car carToDelete = await GetByIdAsync(id);

        _carRepository.Delete(carToDelete);
        await _carRepository.SaveChangesAsync();
    }
}