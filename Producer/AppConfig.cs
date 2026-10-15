namespace Producer
{
    public static class AppConfig
    {
        public const string ConnectionString = "Server=localhost;Database=laboratory_1;User Id=root;Password=12345678;";
        public const string KafkaBootstrapServers = "localhost:9092";
        public const string TopicName = "orders";
        public const int PollingIntervalMs = 3000;
    }
}
