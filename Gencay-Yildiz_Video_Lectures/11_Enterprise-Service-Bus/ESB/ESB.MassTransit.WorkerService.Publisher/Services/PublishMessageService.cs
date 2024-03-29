
using ESB.MassTransit.Shared.Messages;
using MassTransit;

namespace ESB.MassTransit.WorkerService.Publisher.Services
{
    public class PublishMessageService : BackgroundService
    {
        private readonly IPublishEndpoint publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;

            while(true)
            {
                ExampleMessage message = new()
                {
                    Text = $"{++i}. Message from Publisher"
                };

                await publishEndpoint.Publish(message); // Send message to all queues
            }
        }
    }
}
