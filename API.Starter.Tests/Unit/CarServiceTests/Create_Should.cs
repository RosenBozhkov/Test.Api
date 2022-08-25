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

public class Create_Should
{
    private readonly CarService _carService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public Create_Should()
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
    public async Task ReturnCorrectCarResponse_When_CreatedSuccessfully()
    {
        //Arrange
        User owner = new() { Id = It.IsAny<Guid>(), Username = "Roko" };
        Model model = new() { Id = It.IsAny<Guid>(), Name = "Avensis", Make = new() { Id = It.IsAny<Guid>(), Name = "Toyota" } };
        CarCreateRequest validRequest = new() { UserId = owner.Id, MakeName = model.Make.Name, ModelName = model.Name, YearOfCreation = 2010 };

        _userService.Setup(uS => uS.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(owner);
        _modelService.Setup(mS => mS.CreateIfNotExist(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(model);

        CarResponse expectedCar = new() { ModelMakeName = model.Make.Name, ModelName = model.Name, YearOfCreation = 2010, UserName = owner.Username };

        //Act
        CarResponse actualCar = await _carService.CreateAsync(validRequest);

        //Assert
        Assert.Equal(expectedCar.ModelMakeName, actualCar.ModelMakeName);
        Assert.Equal(expectedCar.ModelName, actualCar.ModelName);
        Assert.Equal(expectedCar.YearOfCreation, actualCar.YearOfCreation);
        _userService.Verify(uS => uS.GetUserByIdAsync(It.IsAny<Guid>()), Times.Once);
        _modelService.Verify(mS => mS.CreateIfNotExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionAndCorrectMessage_When_UserNotExist()
    {
        //Arrange
        string expectedMessage = Messages.ResourceNotFound;

        _userService.Setup(uS => uS.GetUserByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException(Messages.ResourceNotFound));

        //Act & Assert
        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _carService.CreateAsync(new CarCreateRequest() { UserId = Guid.NewGuid()}));

        Assert.Equal(expectedMessage, ex.Message);
        _userService.Verify(uS => uS.GetUserByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}