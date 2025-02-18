
using Azure.Core.Serialization;
using Confluent.Kafka;
using System.Text.Json;
using UserService.Data;
using UserService.DTO;
using UserService.Model;

namespace UserService.Service
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "user-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe("user-service-events");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var userData = JsonSerializer.Deserialize<UserCreateDTO>(consumeResult.Message.Value);
                    if (userData != null)
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                            var user = new User
                            {
                                IdentityUserId = userData.Id,
                                DateOfBirth = userData.DateOfBirth,
                                FirstName = userData.FirstName,
                                LastName = userData.LastName,
                                PhoneNumber = userData.Phone,
                                Email = userData.Email,

                            };
                            dbContext.Users.Add(user);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}
