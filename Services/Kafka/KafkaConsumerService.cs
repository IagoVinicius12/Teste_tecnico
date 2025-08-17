using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.Json;
using MongoDB.Entities;

namespace Services.Kafka.Consumer;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(IConfiguration config, ILogger<KafkaConsumerService> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServer = _config["Kafka:BootstrapServers"];
        var topic = _config["Kafka:TopicName"];
        var groupId = _config["Kafka:GroupId"];

        // Esperando o kafka ficar disponivel
        var connected = false;
        while (!connected && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var testProducer = new ProducerBuilder<Null, string>(new ProducerConfig
                {
                    BootstrapServers = bootstrapServer
                }).Build();

                await testProducer.ProduceAsync(topic, new Message<Null, string> { Value = "ping" });
                connected = true;
            }
            catch
            {
                _logger.LogWarning("Kafka ainda não disponível, tentando novamente em 2s...");
                await Task.Delay(2000, stoppingToken);
            }
        }

        // Criando topico automaticamente
        try
        {
            var adminConfig = new AdminClientConfig { BootstrapServers = bootstrapServer };
            using var adminClient = new AdminClientBuilder(adminConfig).Build();

            var metadata = adminClient.GetMetadata(topic, TimeSpan.FromSeconds(5));
            if (!metadata.Topics.Any(t => t.Topic == topic && t.Error.Code == ErrorCode.NoError))
            {
                _logger.LogInformation("Tópico '{topic}' não existe, criando...", topic);
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topic,
                        NumPartitions = 1,
                        ReplicationFactor = 1
                    }
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o tópico");
        }

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServer,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(stoppingToken);
                    var message = cr.Message.Value;

                    if (string.IsNullOrWhiteSpace(message) ||
                        !(message.TrimStart().StartsWith("{") || message.TrimStart().StartsWith("[")))
                    {
                        _logger.LogWarning("Mensagem ignorada (não é JSON): {msg}", message);
                        continue;
                    }

                    var moto = JsonSerializer.Deserialize<Models.MotoModel.Moto>(message);
                    if (moto != null && moto.Year == 2024)
                    {
                        await DB.SaveAsync(moto);
                        _logger.LogInformation("Moto 2024 armazenada: {id}", moto.Identifier);// Salva no banco a mensagem do evento
                    }
                }
                catch (JsonException jex)
                {
                    _logger.LogError(jex, "Erro de JSON ao processar mensagem");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado ao processar mensagem");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
