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

public class Update_Should
{
    private readonly JobService _jobService;

    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly RequestState _requestState;

    public Update_Should()
    {
        _jobRepository = new Mock<IJobRepository>();
        _validatorService = new Mock<IValidatorService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new JobProfile()); })
            .CreateMapper();
        _jobService = new JobService(_jobRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnCorrectJobResponse_When_UpdatedSuccessfully()
    {
        //Arrange
        JobUpdateRequest validRequest = new() { Id = It.IsAny<int>(), Price = 5 };

        if (validRequest.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobUpdateRequest>())).Throws<Exception>();
        }
        Job job = new() { Name = "Oil Change", Price = 10, Id = It.IsAny<int>() };

        _jobRepository.Setup(jR => jR.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(job);
        JobResponse expectedJob = new() { Id = It.IsAny<int>(), Price = 5, Name = "Oil Change" };

        //Act
        JobResponse actualJob = await _jobService.UpdateAsync(validRequest);

        //Assert
        Assert.Equal(expectedJob.Name, actualJob.Name);
        Assert.Equal(expectedJob.Price, actualJob.Price);
        _jobRepository.Verify(jR => jR.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionWithCorrectMessage_When_JobNotFound()
    {
        JobUpdateRequest validRequest = new() { Id = It.IsAny<int>(), Price = 5 };
        string expectedMessage = Messages.ResourceNotFound;

        _jobRepository.Setup(jR => jR.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Job>);

        //Act & Assert
        NotFoundException ex = await Assert.ThrowsAsync<NotFoundException>(
            async () => await _jobService.UpdateAsync(validRequest));

        Assert.Equal(expectedMessage, ex.Message);
        _jobRepository.Verify(jR => jR.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ThrowException_When_PriceNegative()
    {
        //Arrange
        JobUpdateRequest request = new() { Price = -100 };
        string expectedMessage = Messages.ResourceNotFound;

        _jobRepository.Setup(jR => jR.Exists(It.IsAny<string>())).ReturnsAsync(false);

        if (request.Price < 0)
        {
            _validatorService.Setup(vS => vS.Validate(It.IsAny<JobUpdateRequest>())).Throws<Exception>();
        }

        //Act & Assert
        Exception ex = await Assert.ThrowsAsync<Exception>(
            async () => await _jobService.UpdateAsync(request));
    }
}
