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
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace API.Starter.Tests.Unit;

public class CarServiceTests
{
    private readonly CarService _carService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public CarServiceTests()
    {
        _carRepository = new Mock<ICarRepository>();
        _validatorService = new Mock<IValidatorService>();
        _modelService = new Mock<IModelService>();
        _userService = new Mock<IUserService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new CarProfile()); })
            .CreateMapper();
 #pragma warning disable CS8625
        _carService = new CarService(_carRepository.Object, _validatorService.Object, mapper, _requestState, null, _modelService.Object,_userService.Object);
 #pragma warning restore CS8625
    }

    //[Fact]
    //public async Task GetById_WhenGivenValidId_ShouldReturnCar()
    //{
    //    Guid id = Guid.NewGuid();
    //    Car car = new Car
    //    {
    //        Id = id,
    //        Make = "Toyota",
    //        Model = "Corolla",
    //        YearOfCreation = 2000
    //    };
    //    _carRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(car);
    //
    //    CarResponse expectedCar = new CarResponse
    //    {
    //        Id = id,
    //        Make = "Toyota",
    //        Model = "Corolla",
    //        YearOfCreation = 2000
    //    };
    //
    //    CarResponse actualCar = await _carService.GetByIdAsync(id);
    //
    //    Assert.Equal(expectedCar.Id, actualCar.Id);
    //    Assert.Equal(expectedCar.Make, actualCar.Make);
    //    Assert.Equal(expectedCar.Model, actualCar.Model);
    //    Assert.Equal(expectedCar.YearOfCreation, actualCar.YearOfCreation);
    //}

    [Fact]
    public async Task GetById_WhenGivenInvalidId_ThrowException()
    {
        Guid id = Guid.NewGuid();

        _carRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Car?)null);

        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _carService.GetResponseByIdAsync(id));
            
        Assert.Equal("Resource not found", ex.Message);
    }
}