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
        private readonly IConsumer<Ignore, string> _consumer;
        public KafkaConsumerService(IServiceScopeFactory serviceScopeFactory,
                                    IConsumer<Ignore, string> consumer)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _consumer = consumer;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("user-service-events");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1)); // 🔥 Will not block indefinitely

                    if (consumeResult != null && consumeResult.Message != null)
                    {
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

                                await dbContext.Users.AddAsync(user);
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    //_logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Error in Kafka Consumer: {ex.Message}");
                }

                await Task.Delay(1000, stoppingToken); // 🔥 Allows background service to keep running
            }

            _consumer.Close();
        }


    }
}
