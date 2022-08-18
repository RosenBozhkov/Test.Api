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

namespace API.Starter.Tests.Unit.CarServiceTests;

public class GetById_Should
{
    private readonly CarService _carService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public GetById_Should()
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
    public async Task ReturnCar_When_GivenValidId()
    {
        //Arange
        Car car = new()
        {
            Id = It.IsAny<Guid>(),
            YearOfCreation = 2000,
            Model = new Model() { Name = "Corolla", Make = new Make() { Name = "Toyota" } }
        };
        _carRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);

        CarResponse expectedCar = new()
        {
            Id = It.IsAny<Guid>(),
            ModelName = "Corolla",
            ModelMakeName = "Toyota",
            YearOfCreation = 2000
        };

        //Act
        CarResponse actualCar = await _carService.GetResponseByIdAsync(It.IsAny<Guid>());

        //Assert
        Assert.Equal(expectedCar.Id, actualCar.Id);
        Assert.Equal(expectedCar.ModelMakeName, actualCar.ModelMakeName);
        Assert.Equal(expectedCar.ModelName, actualCar.ModelName);
        Assert.Equal(expectedCar.YearOfCreation, actualCar.YearOfCreation);
        _carRepository.Verify(cR => cR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionAndCorrectMessage_When_GivenInvalidId()
    {
        //Arange
        string expectedMessage = Messages.ResourceNotFound;

        _carRepository.Setup(cR => cR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<Car>());

        //Act & Assert
        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _carService.GetResponseByIdAsync(It.IsAny<Guid>()));

        Assert.Equal(expectedMessage, ex.Message);
        _carRepository.Verify(cR => cR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}