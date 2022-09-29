using System;
using System.Collections.Generic;
using Business.Interfaces.v1;
using inacs.v8.nuget.DevAttributes;
using Config.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Metrics;
using Common.Metrics;
using Business.Models.v1;
using Persistence.Interfaces.v1;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using System.Linq;
using System.Threading;

namespace Test.Api.Controllers.v1;

/// <summary>
/// Controller for managing visits
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
public class TestController
{
    private static readonly string ControllerName = typeof(VisitController).FullName!;
    private readonly ThingsContext _context;

    /// <summary>
    /// Default constructor for tests
    /// </summary>

    public TestController(ThingsContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Endpoint for getting all the visits
    /// </summary>
    [Anonymous]
    [HttpPost]
    [Journal]
    [Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseContent<string>), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent<string>> GetAllAsync()
    {
        _context.SaveChanges(true);
        
        return new ResponseContent<string>
        {
            Result = ""
        };
    }
}