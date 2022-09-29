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
using System.Linq;

namespace API.Starter.Tests.Unit.VisitServiceTests;

public class GetById_Should
{
    private readonly VisitService _visitService;

    private readonly Mock<ICarService> _carService;
    private readonly Mock<IVisitRepository> _visitRepository;
    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public GetById_Should()
    {
        _carService = new Mock<ICarService>();
        _visitRepository = new Mock<IVisitRepository>();
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new VisitProfile()); configuration.AddProfile(new JobProfile()); configuration.AddProfile(new CarProfile()); })
            .CreateMapper();
        _visitService = new VisitService(_visitRepository.Object, _validatorService.Object, mapper, _requestState, null, _carService.Object, _jobRepository.Object);
    }

    [Fact]
    public async Task ReturnCorrectVisit_When_GivenValidId()
    {
        //Arrange
        Visit visit = new() {
        
            Id = It.IsAny<Guid>(),
            Car = new Car() { Id = It.IsAny<Guid>(), Modifier = 1.5f, Model = new() {Id = It.IsAny<Guid>(), Name = "Corolla", Make = new(){Id = It.IsAny<Guid>(), Name = "Toyota" } } },
            Jobs = new List<Job>() { new Job() { Id = It.IsAny<int>(), Price = 50 }, new Job() { Id = It.IsAny<int>(), Price = 100 } }
        };

        _visitRepository.Setup(vR => vR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(visit);

        VisitResponse expectedVisit = new() {
        
            Id = It.IsAny<Guid>(),
            Car = new CarResponse() { Id = It.IsAny<Guid>(), Modifier = 1.5f, ModelName = "Corolla", ModelMakeName = "Toyota" },
            Jobs = new List<JobResponse>() { new() { Id = It.IsAny<int>(), Price = 50 }, new() { Id = It.IsAny<int>(), Price = 100 } },
        };

        //Act
        VisitResponse actualVisit = await _visitService.GetResponseByIdAsync(It.IsAny<Guid>());

        //Assert
        Assert.Equal(expectedVisit.Jobs.First().Price, actualVisit.Jobs.First().Price);
        Assert.Equal(expectedVisit.Car.ModelName, actualVisit.Car.ModelName);
        Assert.Equal(expectedVisit.Car.ModelMakeName, actualVisit.Car.ModelMakeName);
        Assert.Equal(expectedVisit.Car.ModelName, actualVisit.Car.ModelName);
        _visitRepository.Verify(vR => vR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionAndCorrectMessage_When_GivenInvalidId()
    {
        //Arange
        string expectedMessage = Messages.ResourceNotFound;

        _visitRepository.Setup(vR => vR.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<Visit>());

        //Act & Assert
        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _visitService.GetResponseByIdAsync(It.IsAny<Guid>()));

        Assert.Equal(expectedMessage, ex.Message);
        _visitRepository.Verify(jR => jR.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
