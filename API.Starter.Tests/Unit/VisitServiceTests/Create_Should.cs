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

public class Create_Should
{
    private readonly VisitService _visitService;
    private readonly IMapper mapper;

    private readonly Mock<ICarService> _carService;
    private readonly Mock<IVisitRepository> _visitRepository;
    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public Create_Should()
    {
        _carService = new Mock<ICarService>();
        _visitRepository = new Mock<IVisitRepository>();
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new VisitProfile()); configuration.AddProfile(new JobProfile()); configuration.AddProfile(new CarProfile()); configuration.AddProfile(new Test.Api.AutoMapper.Profiles.UserProfile()); })
            .CreateMapper();
        _visitService = new VisitService(_visitRepository.Object, _validatorService.Object, mapper, _requestState, null, _carService.Object, _jobRepository.Object);
    }

    [Fact]
    public async Task ReturnCorrectVisitResponse_When_CreatedSuccessfully()
    {
        //Arrange
        User user = new() { Id = It.IsAny<Guid>(), Username = "Roko" };
        Car car = new() { Id = It.IsAny<Guid>(), Modifier = 1.5f, Model = new() { Id = It.IsAny<Guid>(), Name = "Corolla", Make = new() { Id = It.IsAny<Guid>(), Name = "Toyota" } }, User = user };
        Job jobA = new() { Id = It.IsAny<int>(), Price = 50, Name = "Filter change" };
        Job jobB = new() { Id = It.IsAny<int>(), Price = 100, Name = "Oil change" };

        ICollection<Job> jobs = new List<Job>() { jobA, jobB };

        _carService.Setup(cS => cS.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);
        _jobRepository.Setup(jR => jR.GetValidJobs(It.IsAny<List<int>>())).Returns(jobs);

        VisitRegisterRequest validRequest = new();

        VisitResponse expectedVisit = new() { Car = mapper.Map<CarResponse>(car), Jobs = mapper.Map<IList<JobResponse>>(jobs), TotalPrice = 150 * 1.5f, Completion = null };

        //Act
        VisitResponse actualVisit = await _visitService.CreateAsync(validRequest);

        //Assert
        Assert.Equal(expectedVisit.Car.UserName, actualVisit.Car.UserName);
        Assert.Equal(expectedVisit.Car.ModelName, actualVisit.Car.ModelName);
        Assert.Equal(expectedVisit.TotalPrice, actualVisit.TotalPrice);
        Assert.Equal(expectedVisit.Completion, actualVisit.Completion);
        _jobRepository.Verify(jR => jR.GetValidJobs(It.IsAny<List<int>>()), Times.Once);
        _carService.Verify(cS => cS.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}