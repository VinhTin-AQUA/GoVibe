using MassTransit;

namespace GoVibe.API.Messaging.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly IBus bus;

        public RabbitMQService(IBus bus)
        {
            this.bus = bus;
        }

        public async Task SendMessage<T>(T p) where T : notnull
        {
            await bus.Publish(p);
        }

        public async Task<Res> SendWithRequest<Req, Res>(Req request, string queueName)
            where Req : class
            where Res : class
        {
            var uri = new Uri(queueName);
            var response = await bus.Request<Req, Res>(uri, request);

            var res = response.Message;
            return res;
        }
    }
}
