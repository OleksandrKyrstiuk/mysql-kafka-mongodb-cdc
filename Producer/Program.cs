using Producer;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== STARTING PRODUCER SERVICE ===");

        var repository = new OrderRepository(AppConfig.ConnectionString);
        using var kafkaPublisher = new KafkaPublisher(AppConfig.KafkaBootstrapServers, AppConfig.TopicName);

        DateTime lastProcessedTime = DateTime.MinValue;

        while (true)
        {
            try
            {
                var updatedOrders = (await repository.GetUpdatedOrdersAsync(lastProcessedTime)).ToList();

                if (updatedOrders.Any())
                {
                    Console.WriteLine($"\n[MySQL] Знайдено нових/змінених записів: {updatedOrders.Count}");

                    foreach (var order in updatedOrders)
                    {
                        var result = await kafkaPublisher.PublishOrderAsync(order);

                        Console.WriteLine($"[Kafka Sent] Key (idOrders): {order.IdOrders} | Partition: {result.Partition} | Offset: {result.Offset}");

                        if (order.UpdatedAt > lastProcessedTime)
                        {
                            lastProcessedTime = order.UpdatedAt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Помилка виконання: {ex.Message}");
            }

            await Task.Delay(AppConfig.PollingIntervalMs);
        }
    }
}