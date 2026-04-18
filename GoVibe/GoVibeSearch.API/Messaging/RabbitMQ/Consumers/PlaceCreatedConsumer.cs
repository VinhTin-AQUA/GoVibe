using Contracts.Places;
using MassTransit;

namespace GoVibeSearch.API.Messaging.RabbitMQ.Consumers
{
    public class PlaceCreatedConsumer : IConsumer<PlaceCreatedEvent>
    {
        public async Task Consume(ConsumeContext<PlaceCreatedEvent> context)
        {
            var prod = context.Message;
            Console.WriteLine($"Place Consumer recieve: {prod.Id} - {prod.Name} ");
            
            await Task.CompletedTask;
        }
    }
}