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

public class GetById_Should
{
    private readonly JobService _jobService;

    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public GetById_Should()
    {
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new JobProfile()); })
            .CreateMapper();
        _jobService = new JobService(_jobRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnCorrectCar_When_GivenValidId()
    {
        //Arange
        Job job = new()
        {
            Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 100
        };
        _jobRepository.Setup(cR => cR.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(job);

        JobResponse expectedJob = new()
        {
            Id = It.IsAny<int>(),
            Name = "Oil Change",
            Price = 100
        };

        //Act
        JobResponse actualJob = await _jobService.GetResponseByIdAsync(It.IsAny<int>());

        //Assert
        Assert.Equal(expectedJob.Id, actualJob.Id);
        Assert.Equal(expectedJob.Name, actualJob.Name);
        Assert.Equal(expectedJob.Price, actualJob.Price);
        _jobRepository.Verify(jR => jR.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionAndCorrectMessage_When_GivenInvalidId()
    {
        //Arange
        string expectedMessage = Messages.ResourceNotFound;

        _jobRepository.Setup(jR => jR.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Job>());

        //Act & Assert
        NotFoundException ex =
            await Assert.ThrowsAsync<NotFoundException>(async () => await _jobService.GetResponseByIdAsync(It.IsAny<int>()));

        Assert.Equal(expectedMessage, ex.Message);
        _jobRepository.Verify(jR => jR.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }
}
