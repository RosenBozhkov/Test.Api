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

public class Create_Should
{
    private readonly JobService _jobService;

    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public Create_Should()
    {
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new JobProfile()); })
            .CreateMapper();
        _jobService = new JobService(_jobRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnCorrectJobResponse_When_CreatedSuccessfully()
    {
        //Arrange
        JobCreateRequest validRequest = new() { Name = "Oil Change", Price = 100 };

        _jobRepository.Setup(jR => jR.Exists(It.IsAny<string>())).ReturnsAsync(false);

        if (validRequest.Name.Length is < 5 or > 35 || validRequest.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobCreateRequest>())).Throws<Exception>();
        }

        JobResponse expectedJob = new() { Name = "Oil Change", Price = 100 };

        //Act
        JobResponse actualJob = await _jobService.CreateAsync(validRequest);
        
        ////Assert
        Assert.Equal(expectedJob.Name, actualJob.Name);
        Assert.Equal(expectedJob.Price, actualJob.Price);
        _jobRepository.Verify(jR => jR.Exists(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ThrowException_When_PriceNegative()
    {
        //Arrange
        JobCreateRequest request = new() { Name = "Oil Change", Price = -100 };

        _jobRepository.Setup(jR => jR.Exists(It.IsAny<string>())).ReturnsAsync(false);

        if (request.Name.Length is < 5 or > 35 || request.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobCreateRequest>())).Throws<Exception>();
        }

        //Act & Assert
        Exception ex = await Assert.ThrowsAsync<Exception>(
            async () => await _jobService.CreateAsync(request));
    }
    
    [Fact]
    public async Task ThrowException_When_NameNotValid()
    {
        //Arrange
        JobCreateRequest request = new() { Name = "Oi", Price = 100 };

        _jobRepository.Setup(jR => jR.Exists(It.IsAny<string>())).ReturnsAsync(false);

        if (request.Name.Length is < 5 or > 35 || request.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobCreateRequest>())).Throws<Exception>();
        }

        //Act & Assert
        Exception ex = await Assert.ThrowsAsync<Exception>(
            async () => await _jobService.CreateAsync(request));
    }
    
    [Fact]
    public async Task ThrowExceptionWithCorrectMessage_When_NameIsAlreadyInUse()
    {
        //Arrange
        JobCreateRequest validRequest = new() { Name = "Oil Change", Price = 100 };
        string expectedMessage = "Job with that name already exists.";

        _jobRepository.Setup(jR => jR.Exists(It.IsAny<string>())).ReturnsAsync(true);

        if (validRequest.Name.Length is < 5 or > 35 || validRequest.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobCreateRequest>())).Throws<Exception>();
        }

        //Act & Assert
        InvalidNameException ex = await Assert.ThrowsAsync<InvalidNameException>(
            async () => await _jobService.CreateAsync(validRequest));

        Assert.Equal(expectedMessage, ex.Message);
        _jobRepository.Verify(jR => jR.Exists(It.IsAny<string>()), Times.Once);
    }
}

