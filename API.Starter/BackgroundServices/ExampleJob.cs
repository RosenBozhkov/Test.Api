using Quartz;
using System;
using System.Threading.Tasks;
using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.Logging;

#warning delete me if not needed(keeping it for now)
namespace Test.Api.BackgroundServices;

[Developer("Vasil Egov", "v.egov@gmail.com")]
internal class ExampleJob : IJob
{
    //Automapped by library
    public string Name { get; set; } = string.Empty;
    private readonly ILogger<ExampleJob> _logger;

    public ExampleJob(ILogger<ExampleJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{Date} Greetings {Name} from HelloJob!", DateTime.Now, Name);
        return Task.CompletedTask;
    }
}