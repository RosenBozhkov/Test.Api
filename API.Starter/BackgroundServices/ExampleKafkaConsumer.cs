using Confluent.Kafka;
using inacs.v8.nuget.EnvHelper;
using inacs.v8.nuget.Kafka.Consumer;
using inacs.v8.nuget.Kafka.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Api.BackgroundServices;

/// <summary>
/// 
/// </summary>
public class ExampleKafkaConsumer : BackgroundService
{
    private readonly ConsumerConfig consumerConfig;
    private readonly ILogger<ExampleKafkaConsumer> logger;
    private IConsumer<string, string> consumer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public ExampleKafkaConsumer(IConfiguration configuration, ILogger<ExampleKafkaConsumer> logger)
    {
        this.logger = logger;

        consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Brokers").Value,
            EnableAutoCommit = false,
            ClientId = $"exampleconsumer:{EnvironmentHelper.TryGetExternalIp()}:{EnvironmentHelper.TryGetPort()}",
            GroupId = EnvironmentHelper.TryGetServiceName(),
            AllowAutoCreateTopics = true,
            IsolationLevel = IsolationLevel.ReadCommitted,
            //Debug = Configuration.GetSection(KafkaOptions.CONFIGURATION_KEY).GetSection("Debug").Value,
        };

        consumer = BuildConsumer();
        consumer.Subscribe("asdfg");
    }

/// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () => 

        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ConsumeResult<string, string> consumedMessage = consumer.Consume(stoppingToken);
                    KafkaConsumeMessage message = consumedMessage.ToKafkaMessage();

                    Console.WriteLine(JsonConvert.SerializeObject(message, Formatting.Indented));

                    // TODO: process(message);

                    consumer.Commit(consumedMessage);
                }
                catch (KafkaException ex)
                {
                    HandleKafkaException(ex);
                }
            }
        }
        , stoppingToken);
        return Task.CompletedTask;
    }

    private IConsumer<string, string> BuildConsumer()
    {
        return new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    private void HandleKafkaException(KafkaException ex)
    {
        logger.LogError(ex, ex.Message);
        if (ex.Error.IsFatal)
        {
            logger.LogError("Fatal error; attempting to recover by rebuilding the consumer.");
            if (consumer != null)
            {
                consumer?.Dispose();
                consumer = null;
            }
            consumer = BuildConsumer();
        }
    }
}