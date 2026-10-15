using System;
using System.Threading;
using Confluent.Kafka;

public class KafkaConsumerService : IDisposable
{
    private readonly IConsumer<int, string> _consumer;

    public KafkaConsumerService(string bootstrapServers, string groupId, string topicName)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<int, string>(config).Build();
        _consumer.Subscribe(topicName);
    }

    public ConsumeResult<int, string> Consume(CancellationToken cancellationToken)
    {
        return _consumer.Consume(cancellationToken);
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}