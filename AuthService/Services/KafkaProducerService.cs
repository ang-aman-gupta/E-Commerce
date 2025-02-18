using Confluent.Kafka;
using System.Collections.Concurrent;

namespace AuthService.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<string,string>(config).Build();

        }

        public async Task SendMessageAsync(string topic, string message)
        {
            await _producer.ProduceAsync(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        }
    }
}
