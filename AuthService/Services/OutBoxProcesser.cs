
using AuthService.DBContext;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class OutBoxProcesser : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly KafkaProducerService _kafkaProducerService;

        public OutBoxProcesser(IServiceScopeFactory serviceScopeFactory, KafkaProducerService kafkaProducerService )
        {
            _kafkaProducerService = kafkaProducerService;
            _scopeFactory = serviceScopeFactory;    
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope  = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AuthServiceDbContext>();

                    var pendingEvents = await (from outBox in dbContext.OutBoxEvents 
                                               where !outBox.Processed
                                               select outBox).ToListAsync();
                    foreach (var pendingEvent in pendingEvents)
                    {
                        await _kafkaProducerService.SendMessageAsync("user-service-events", pendingEvent.Payload);
                        pendingEvent.Processed = true;
                    }
                    await dbContext.SaveChangesAsync();
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
