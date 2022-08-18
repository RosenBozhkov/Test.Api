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
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace API.Starter.Tests.Unit.MakeServiceTests;

public class GetById_Should
{
    private readonly MakeService _makeService;

    private readonly Mock<IMakeRepository> _makeRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public GetById_Should()
    {
        _makeRepository = new Mock<IMakeRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new MakeProfile()); })
            .CreateMapper();
        _makeService = new MakeService(_makeRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnMake_WhenGivenValidId()
    {
        //Arange
        Make make = new()
        {
            Id = It.IsAny<Guid>(),
            Name = "Toyota"
        };
        _makeRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(make);

        MakeResponse expectedMake = new()
        {
            Id = It.IsAny<Guid>(),
            Name = "Toyota"
        };

        //Act
        MakeResponse actualMake = await _makeService.GetByIdAsync(It.IsAny<Guid>());

        //Assert
        Assert.Equal(expectedMake.Id, actualMake.Id);
        Assert.Equal(expectedMake.Name, actualMake.Name);
        _makeRepository.Verify(cR => cR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionAndCorrectMessage_WhenGivenInvalidId()
    {
        //Arange
        string expectedMessage = Messages.ResourceNotFound;

        _makeRepository.Setup(mR => mR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<Make>());

        //Act & Assert
        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _makeService.GetByIdAsync(It.IsAny<Guid>()));

        Assert.Equal(expectedMessage, ex.Message);
        _makeRepository.Verify(mR => mR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
