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

namespace API.Starter.Tests.Unit.JobServiceTests;

public class GetAll_Should
{
    private readonly JobService _jobService;

    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public GetAll_Should()
    {
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new JobProfile()); })
            .CreateMapper();
        _jobService = new JobService(_jobRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnCorrectCollection()
    {
        //Arange
        IList<Job> jobs = new List<Job>() { new()
        {
            Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 100
        }, new()
        {
            Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 88
        } };

        _jobRepository.Setup(jR => jR.GetAllAsync()).ReturnsAsync(jobs);

        IList<JobResponse> expectedJobs = new List<JobResponse>() { new()
        {
            Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 100
        }, new()
        {
             Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 88
        } };

        //Act
        IList<JobResponse> actualJobs = await _jobService.GetAllAsync();

        //Assert
        Assert.Equal(expectedJobs.Count, actualJobs.Count);
        Assert.Equal(expectedJobs[0].Name, actualJobs[0].Name);
        Assert.Equal(expectedJobs[0].Price, actualJobs[0].Price);
        Assert.Equal(expectedJobs[1].Name, actualJobs[1].Name);
        Assert.Equal(expectedJobs[1].Price, actualJobs[1].Price);
        _jobRepository.Verify(jR => jR.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ReturnEmpty_When_NotFound()
    {
        //Arange
        IList<Job> jobs = new List<Job>();

        _jobRepository.Setup(jR => jR.GetAllAsync()).ReturnsAsync(jobs);

        IList<JobResponse> expectedJobs = new List<JobResponse>();

        //Act
        IList<JobResponse> actualJobs = await _jobService.GetAllAsync();

        //Assert
        Assert.Equal(expectedJobs.Count, actualJobs.Count);
        _jobRepository.Verify(jR => jR.GetAllAsync(), Times.Once);
    }
}