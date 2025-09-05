using MassTransit;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Domain.Models;

public class MotorcycleCreatedConsumer : IConsumer<MotorcycleCreatedEvent>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MotorcycleCreatedConsumer> _logger;
    public MotorcycleCreatedConsumer(ApplicationDbContext dbContext, ILogger<MotorcycleCreatedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MotorcycleCreatedEvent> context)
    {
       
        var message = context.Message;
        if (message.Year == 2024)
        {
            var notification = new MotorcycleNotification
            {
                Id = Guid.NewGuid(),
               Identifier = message.Identifier,
                Year = message.Year,
                Model = message.Model,
                Plate = message.Plate
            };
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("MotorcycleCreated received: {Identifier}, {Model}, {Plate}, {Year}",
                                    message.Identifier, message.Model, message.Plate, message.Year);
        }
        else
        {
            _logger.LogWarning("Message ignored because the Year is not 2024. Identifier={Identifier}", message.Identifier);
        }
    }
}
