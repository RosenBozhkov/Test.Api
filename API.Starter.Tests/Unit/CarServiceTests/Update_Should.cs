using Business.Models.v1;
using Common.Exceptions;
using inacs.v8.nuget.Core.Models;
using Moq;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Xunit;
using Business.Validators.v1;
using Common.Resources;
using System;
using System.Threading.Tasks;
using Test.Api.AutoMapper.Profiles;
using AutoMapper;
using Business.Implementations.v1;
using Business.Interfaces.v1;

namespace API.Starter.Tests.Unit.CarServiceTests;

public class Update_Should
{
    private readonly CarService _carService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public Update_Should()
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
    
    public async Task ReturnCorrectCarResponse_When_UpdatedSuccessfully()
    {
        //Arrange
        User owner = new() { Id = It.IsAny<Guid>(), Username = "Roko" };
        Model model = new() { Id = It.IsAny<Guid>(), Name = "Avensis", Make = new() { Id = It.IsAny<Guid>(), Name = "Toyota" } };
        CarUpdateRequest validRequest = new() { UserId = owner.Id, Modifier = 1.6f };

        Car car = new() { Id = It.IsAny<Guid>(), Model = model, Modifier = 2};

        _carRepository.Setup(cR => cR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);

        CarResponse expectedCar = new() { ModelMakeName = model.Make.Name, ModelName = model.Name, YearOfCreation = 2010,
            UserName = owner.Username, Modifier = 1.6f };

        //Act
        CarResponse actualCar = await _carService.UpdateAsync(validRequest);

        //Assert
        Assert.Equal(expectedCar.ModelMakeName, actualCar.ModelMakeName);
        Assert.Equal(expectedCar.ModelName, actualCar.ModelName);
        Assert.Equal(expectedCar.Modifier, actualCar.Modifier);
        _carRepository.Verify(cR => cR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    public async Task ReturnCorrectCarResponse_When_UpdatedModifierSuccessfully()
    {
        //Arrange
        Model model = new() { Id = It.IsAny<Guid>(), Name = "Avensis", Make = new() { Id = It.IsAny<Guid>(), Name = "Toyota" } };
        CarUpdateRequest validRequest = new() { Modifier = 1.6f };

        Car car = new() { Id = It.IsAny<Guid>(), Model = model, Modifier = 2 };

        _carRepository.Setup(cR => cR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);

        CarResponse expectedCar = new() { ModelMakeName = model.Make.Name, ModelName = model.Name, YearOfCreation = 2010,
            Modifier = 1.6f };

        //Act
        CarResponse actualCar = await _carService.UpdateAsync(validRequest);

        //Assert
        Assert.Equal(expectedCar.ModelMakeName, actualCar.ModelMakeName);
        Assert.Equal(expectedCar.ModelName, actualCar.ModelName);
        Assert.Equal(expectedCar.Modifier, actualCar.Modifier);
        _carRepository.Verify(cR => cR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    //TODO: nqma li tolkoz kv oda se oburka
    public async Task ThrowExceptionWithCorrectMessage_When_CarNotFound()
    {

    }
}