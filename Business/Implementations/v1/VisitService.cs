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
using System.Linq;

namespace Business.Implementations.v1;

/// <summary>
/// Visit service
/// </summary>
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICarService _carService;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<VisitService> _logger;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="visitRepository"></param>
    /// <param name="jobRepository"></param>
    /// <param name="carService"></param>
    /// <param name="validatorService"></param>
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    public VisitService(
        IVisitRepository visitRepository,
        IValidatorService validatorService,
        IMapper mapper,
        RequestState requestState,
        ILogger<VisitService> logger,
        ICarService carService,
        IJobRepository jobRepository)
    {
        _visitRepository = visitRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
        _carService = carService;
        _jobRepository = jobRepository;
    }

    /// <summary>
    /// Get visit by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task<VisitResponse> GetByIdAsync(Guid id)
    {
        Visit visit = await _visitRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        var result = _mapper.Map<VisitResponse>(visit);
        return result;
    }

    /// <summary>
    /// Get all visits
    /// </summary>
    public async Task<IList<VisitResponse>> GetAllAsync()
    {
        var visits = await _visitRepository.GetAllAsync();

        //TODO: ADD PAGE NESHTOSI
        var result = _mapper.Map<IList<VisitResponse>>(visits);

        return result;
    }

    /// <summary>
    /// Create a visit
    /// </summary>
    /// <param name="model"></param>
    public async Task<VisitResponse> CreateAsync(VisitRegisterRequest model)
    {
        Car car = await _carService.GetByIdAsync(model.CarId);

        ICollection<Job> jobs = _jobRepository.GetValidJobs(model.JobIds);

        float sum = jobs.Select(j => j.Price).Sum();
        float totalSum = sum * car.Modifier;

        Visit visitToCreate = new() { Car = car, Jobs = jobs, TotalPrice = totalSum };

        _visitRepository.Add(visitToCreate);
        await _visitRepository.SaveChangesAsync();

        return _mapper.Map<VisitResponse>(visitToCreate);
    }

    /// <summary>
    /// Update a visit
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    public async Task<VisitResponse> UpdateAsync(VisitFinishRequest model)
    {
        _validatorService.Validate(model);

        Visit visit = await _visitRepository.GetByIdAsync(model.Id)
            ?? throw new NotFoundException(Messages.ResourceNotFound);

        ValidateAdditionalPrice(visit.TotalPrice, model.AdditionalCost);

        BindFromRequest(model, visit);
        await _visitRepository.SaveChangesAsync();

        return _mapper.Map<VisitResponse>(visit);
    }


    /// <summary>
    /// Delete a visit by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        await _visitRepository.DeleteByIdAsync(id);
        await _visitRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Additional price cannot exceed 10% of the total
    /// </summary>
    /// <param name="price"></param>
    /// <param name="additional"></param>
    /// <exception cref="NotFoundException"></exception>
    private static void ValidateAdditionalPrice(float price, int additional)
    {
        if (price / 10 <= additional)
        {
            throw new NotFoundException("Price cannot exceed 10% of the total price of a given Visit to the Shop");
        }
    }

    /// <summary>
    /// Updates DB Visit from the Request
    /// </summary>
    /// <param name="model"></param>
    /// <param name="visit"></param>
    private static void BindFromRequest(VisitFinishRequest model, Visit visit)
    {
        visit.TotalPrice += model.AdditionalCost;
        visit.Completion = DateTime.UtcNow;
    }
}