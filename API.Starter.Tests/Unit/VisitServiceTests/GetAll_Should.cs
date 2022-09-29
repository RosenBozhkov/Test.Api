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

namespace API.Starter.Tests.Unit.VisitServiceTests;

public class GetAll_Should
{
    private readonly VisitService _visitService;

    private readonly Mock<ICarService> _carService;
    private readonly Mock<IVisitRepository> _visitRepository;
    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public GetAll_Should()
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
    public async Task ReturnCorrectCollection()
    {
        //Arrange
        IList<Visit> visits = new List<Visit>() { new()
        {
            Id = It.IsAny<Guid>(),
            Car = new Car() { Id = It.IsAny<Guid>(), Modifier = 1.5f, Model = new() {Id = It.IsAny<Guid>(), Name = "Corolla", Make = new(){Id = It.IsAny<Guid>(), Name = "Toyota" } } },
            Jobs = new List<Job>() { new Job() { Id = It.IsAny<int>(), Price = 50 }, new Job() { Id = It.IsAny<int>(), Price = 100 } },
        } };

        _visitRepository.Setup(vR => vR.GetAllAsync()).ReturnsAsync(visits);

        IList<VisitResponse> expectedVisits = new List<VisitResponse>() { new()
        {
            Id = It.IsAny<Guid>(),
            Car = new CarResponse() { Id = It.IsAny<Guid>(), Modifier = 1.5f, ModelName = "Corolla", ModelMakeName = "Toyota" },
            Jobs = new List<JobResponse>() { new() { Id = It.IsAny<int>(), Price = 50 }, new() { Id = It.IsAny<int>(), Price = 100 } },
        } };

        //Act
        IList<VisitResponse> actualVisits = await _visitService.GetAllAsync();

        //Assert
        Assert.Equal(expectedVisits.Count, actualVisits.Count);
        Assert.Equal(expectedVisits[0].Car.ModelName, actualVisits[0].Car.ModelName);
        Assert.Equal(expectedVisits[0].Car.ModelMakeName, actualVisits[0].Car.ModelMakeName);
        Assert.Equal(expectedVisits[0].Car.ModelName, actualVisits[0].Car.ModelName);
    }

    [Fact]
    public async Task ReturnEmpty_When_NotFound()
    {
        //Arange
        IList<Visit> visits = new List<Visit>();

        _visitRepository.Setup(vR => vR.GetAllAsync()).ReturnsAsync(visits);

        int expectedVisits = 0;

        //Act
        IList<VisitResponse> actualVisits = await _visitService.GetAllAsync();

        //Assert
        Assert.Equal(expectedVisits, actualVisits.Count);
        _visitRepository.Verify(vR => vR.GetAllAsync(), Times.Once);
    }
}