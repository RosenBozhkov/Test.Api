using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.Kafka.Producer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Test.Api.Controllers.MaybeCheckLater;

/// <summary>
/// Controller for managing cars
/// </summary>
[ApiExplorerSettings(GroupName = "1.0")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class KafkaController : ControllerBase
{
    private const string Topic = "asdfg";
    private readonly StandardKafkaProducer standardKafkaProducer;
    private readonly TransactionalKafkaProducer transactionalKafkaProducer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="standardKafkaProducer"></param>
    /// <param name="transactionalKafkaProducer"></param>
    public KafkaController(StandardKafkaProducer standardKafkaProducer, TransactionalKafkaProducer transactionalKafkaProducer)
    {
        this.standardKafkaProducer = standardKafkaProducer;
        this.transactionalKafkaProducer = transactionalKafkaProducer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    [HttpPost("produce")]
    [Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
    [ProducesResponseType(typeof(ResponseContent), 200)]
    [ProducesDefaultResponseType(typeof(ResponseContent))]
    public async Task<ResponseContent> ProduceAsync()
    {
        await standardKafkaProducer.ProduceAsync(KafkaProduceMessageFactory.Message(Topic, "aaaaaa some payload aaa e e e e e", "1.2.3.4.5.6", "6.5.4.3.2.1.1"));
        standardKafkaProducer.Produce(KafkaProduceMessageFactory.Message(Topic, "not awaiting aaaaaa some payload aaa e e e e e", "1.2.3.4.5.6", "6.5.4.3.2.1.1"));
        try
        {
            transactionalKafkaProducer.BeginTransaction();
            await transactionalKafkaProducer.ProduceAsync(KafkaProduceMessageFactory.Message(Topic, "1transactional aaaaaa some payload aaa e e e e e", "1.2.3.4.5.6", "6.5.4.3.2.1.1"));
            await transactionalKafkaProducer.ProduceAsync(KafkaProduceMessageFactory.Message(Topic, "2transactional aaaaaa some payload aaa e e e e e", "1.2.3.4.5.6", "6.5.4.3.2.1.1"));
            await transactionalKafkaProducer.ProduceAsync(KafkaProduceMessageFactory.Message(Topic, "3transactional aaaaaa some payload aaa e e e e e", "1.2.3.4.5.6", "6.5.4.3.2.1.1"));
            transactionalKafkaProducer.CommitTransaction();
        }
        catch (System.Exception)
        {
            transactionalKafkaProducer.AbortTransaction();
            throw;
        }

        return new ResponseContent();
    }
}