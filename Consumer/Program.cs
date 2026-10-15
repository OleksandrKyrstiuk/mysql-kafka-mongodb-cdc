using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== STARTING CONSUMER SERVICE ===");

        var mongoRepository = new MongoOrderRepository(AppConfig.MongoConnectionString, AppConfig.MongoDatabaseName, AppConfig.MongoCollectionName);
        using var kafkaConsumer = new KafkaConsumerService( AppConfig.KafkaBootstrapServers, AppConfig.ConsumerGroupId, AppConfig.TopicName);

        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        Console.WriteLine($"[Kafka Subscribed] Очікування повідомлень з топіка '{AppConfig.TopicName}'...");

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var consumeResult = kafkaConsumer.Consume(cts.Token);

                if (consumeResult?.Message == null) continue;

                Console.WriteLine($"\n[Kafka Received] Key (idOrders): {consumeResult.Message.Key}");

                var receivedOrder = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);
                if (receivedOrder == null) continue;

                var mongoDoc = MongoOrder.FromOrder(receivedOrder);
                bool isInserted = await mongoRepository.UpsertOrderAsync(mongoDoc);

                if (isInserted)
                {
                    Console.WriteLine($"[MongoDB INSERT] Створено новий документ з _id: {mongoDoc.IdOrders}");
                }
                else
                {
                    Console.WriteLine($"[MongoDB UPDATE] Оновлено існуючий документ з _id: {mongoDoc.IdOrders}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\n[Consumer] Роботу сервісу завершено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Помилка обробки: {ex.Message}");
        }
    }
}