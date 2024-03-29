using ESB.MassTransit.Shared.Messages;
using MassTransit;

namespace ESB.MassTransit.WorkerService.Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Received: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
