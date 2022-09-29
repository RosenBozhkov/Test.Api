using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces.v1;
using Business.Models.v1;
using Common.Exceptions;
using Common.Resources;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.Core.Models;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Business.Validators.v1;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ErrorOr;

namespace Business.Implementations.v1;

/// <summary>
/// Job service
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<JobService> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="jobRepository"></param>
    /// <param name="validatorService"></param>
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public JobService(IJobRepository jobRepository, IValidatorService validatorService, IMapper mapper,
        RequestState requestState, ILogger<JobService> logger)
    {
        _jobRepository = jobRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
    }

    /// <summary>
    /// Get job by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task<JobResponse> GetResponseByIdAsync(int id)
    {
        Job job = await GetByIdAsync(id);

        var result = _mapper.Map<JobResponse>(job);
        return result;
    }

    /// <summary>
    /// Get all jobs
    /// </summary>
    public async Task<IList<JobResponse>> GetAllAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();

        var result = _mapper.Map<IList<JobResponse>>(jobs);

        return result;
    }

    /// <summary>
    /// Create a job
    /// </summary>
    /// <param name="model"></param>
    public async Task<JobResponse> CreateAsync(JobCreateRequest model)
    {
        _validatorService.Validate(model);

        await ValidateNameNotExistAsync(model.Name);

        Job jobToCreate = _mapper.Map<Job>(model);
        _jobRepository.Add(jobToCreate);
        await _jobRepository.SaveChangesAsync();

        return _mapper.Map<JobResponse>(jobToCreate);
    }

    /// <summary>
    /// Update a job
    /// </summary>
    /// <param name="model"></param>
    public async Task<JobResponse> UpdateAsync(JobUpdateRequest model)
    {
        _validatorService.Validate(model);

        Job job = await GetByIdAsync(model.Id);

        job.Price = model.Price;
        await _jobRepository.SaveChangesAsync();

        return _mapper.Map<JobResponse>(job);
    }

    /// <summary>
    /// Delete a job by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(int id)
    {
        Job jobToDelete = await GetByIdAsync(id);

        _jobRepository.Delete(jobToDelete);
        await _jobRepository.SaveChangesAsync();
    }

    private async Task<Job> GetByIdAsync(int id)
    {
        return await _jobRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(Messages.ResourceNotFound);
    }

    private async Task ValidateNameNotExistAsync(string name)
    {
        bool exists = await _jobRepository.Exists(name);

        if (exists)
        {
            throw new InvalidNameException("Job with that name already exists.");
        }
    }
}