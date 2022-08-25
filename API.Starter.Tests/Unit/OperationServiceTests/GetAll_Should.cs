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

namespace API.Starter.Tests.Unit.OperationServiceTests;

public class GetAll_Should
{
    private readonly JobService _jobService;

    private readonly Mock<ICarRepository> _carRepository;
    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IValidatorService> _validatorService;
    private readonly Mock<IModelService> _modelService;
    private readonly Mock<IUserService> _userService;
    private readonly RequestState _requestState;

    public GetAll_Should()
    {
        _jobRepository = new Mock<IJobRepository>();
        _carRepository = new Mock<ICarRepository>();
        _validatorService = new Mock<IValidatorService>();
        _modelService = new Mock<IModelService>();
        _userService = new Mock<IUserService>();
        _requestState = new RequestState(Guid.NewGuid());
        IMapper mapper = new MapperConfiguration(configuration => { configuration.AddProfile(new CarProfile()); })
            .CreateMapper();
        _jobService = new JobService(_jobRepository.Object, _validatorService.Object, mapper, _requestState, null);
    }

    [Fact]
    public async Task ReturnCorrectCollection()
    { 

    }
}