using Confluent.Kafka;
using System.Text.Json;

public static class KafkaProducer
{
    private static readonly ProducerConfig config = new ProducerConfig
    {
        BootstrapServers = "localhost:7063"
    };

    public static void Produce(string topic, object message)
    {
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var jsonMessage = JsonSerializer.Serialize(message);
        producer.Produce(topic, new Message<Null, string> { Value = jsonMessage });
    }
}
