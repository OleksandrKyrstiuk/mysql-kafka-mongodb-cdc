public static class AppConfig
{
    public const string KafkaBootstrapServers = "localhost:9092";
    public const string TopicName = "orders";
    public const string ConsumerGroupId = "orders-consumer-group";

    public const string MongoConnectionString = "mongodb://localhost:27017";
    public const string MongoDatabaseName = "logistics_db";
    public const string MongoCollectionName = "orders";
}