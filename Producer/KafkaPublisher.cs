using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaPublisher : IDisposable
{
    private readonly IProducer<int, string> _producer;
    private readonly string _topicName;

    public KafkaPublisher(string bootstrapServers, string topicName)
    {
        _topicName = topicName;
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<int, string>(config).Build();
    }

    public async Task<DeliveryResult<int, string>> PublishOrderAsync(Order order)
    {
        string jsonValue = JsonSerializer.Serialize(order);

        var message = new Message<int, string>
        {
            Key = order.IdOrders,
            Value = jsonValue
        };

        return await _producer.ProduceAsync(_topicName, message);
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}