using Confluent.Kafka;
using System.Text.Json;

namespace Services.Kafka.Producer;

public class KafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(IConfiguration config)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        _topic = config["Kafka:TopicName"];
    }

    public async Task PublishMotoCreatedAsync(Models.MotoModel.Moto moto)
    {
        // Quando a moto for do ano 2024 vai ocorrer e será feito o cadastro da mensagem na tabela EventMoto
        var message = JsonSerializer.Serialize(moto);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        Console.WriteLine($"Evento publicado: Moto {moto.Identifier}");
    }
}
