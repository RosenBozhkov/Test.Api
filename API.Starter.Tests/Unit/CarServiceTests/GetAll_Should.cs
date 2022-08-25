using System;
using System.Threading.Tasks;
using Test.Api.AutoMapper.Profiles;
using AutoMapper;
using Business.Implementations.v1;
using Business.Interfaces.v1;
using Business.Models.v1;
using Common.Exceptions;
using inacs.v8.nuget.Core.Models;
using Moq;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Xunit;
using Business.Validators.v1;
using Common.Resources;
using System.Collections.Generic;

namespace API.Starter.Tests.Unit.CarServiceTests;

public class GetAll_Should
{
    private readonly CarService _carService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public GetAll_Should()
    {
        _carRepository = new Mock<ICarRepository>();
        _validatorService = new Mock<IValidatorService>();
        _modelService = new Mock<IModelService>();
        _userService = new Mock<IUserService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new CarProfile()); })
            .CreateMapper();
        _carService = new CarService(_carRepository.Object, _validatorService.Object, mapper, _requestState, null, _modelService.Object, _userService.Object);
    }

    [Fact]
    public async Task ReturnCorrectCollection()
    {
        //Arange
        IList<Car> cars = new List<Car>() { new()
        {
            Id = It.IsAny<Guid>(),
            YearOfCreation = 2000,
            Model = new Model() { Name = "Corolla", Make = new Make() { Name = "Toyota" } }
        }, new()
        {
            Id = It.IsAny<Guid>(),
            YearOfCreation = 2010,
            Model = new Model() { Name = "Accord", Make = new Make() { Name = "Honda" } }
        } };

        _carRepository.Setup(cR => cR.GetAllAsync()).ReturnsAsync(cars);

        IList<CarResponse> expectedCars = new List<CarResponse>() { new()
        {
             Id = It.IsAny<Guid>(),
            ModelName = "Corolla",
            ModelMakeName = "Toyota",
            YearOfCreation = 2000
        }, new()
        {
             Id = It.IsAny<Guid>(),
            ModelName = "Accord",
            ModelMakeName = "Honda",
            YearOfCreation = 2010
        } };

        //Act
        IList<CarResponse> actualCars = await _carService.GetAllAsync();

        //Assert
        Assert.Equal(expectedCars.Count, actualCars.Count);
        Assert.Equal(expectedCars[0].ModelMakeName, actualCars[0].ModelMakeName);
        Assert.Equal(expectedCars[1].ModelName, actualCars[1].ModelName);
        Assert.Equal(expectedCars[1].YearOfCreation, actualCars[1].YearOfCreation);
        _carRepository.Verify(cR => cR.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ReturnEmpty_When_NotFound()
    {
        //Arange
        IList<Car> cars = new List<Car>();

        _carRepository.Setup(cR => cR.GetAllAsync()).ReturnsAsync(cars);

        IList<CarResponse> expectedCars = new List<CarResponse>();

        //Act
        IList<CarResponse> actualCars = await _carService.GetAllAsync();

        //Assert
        Assert.Equal(expectedCars.Count, actualCars.Count);
        _carRepository.Verify(cR => cR.GetAllAsync(), Times.Once);
    }
}