using MassTransit;
using Shared.RequestResponseMessage;

namespace Consumer.Consumers
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            Console.WriteLine($"Request received : {context.Message.Text}");
            await context.RespondAsync<ResponseMessage>(new()
            {
                Text = $"Response to request: {context.Message.Text}"
            });
        }
    }
}
