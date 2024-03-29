using ESB.MassTransit.Shared.Messages;
using MassTransit;

namespace ESB.MassTransit.Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Received Message: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
