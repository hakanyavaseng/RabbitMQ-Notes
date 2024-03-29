using ESB.MassTransit.WorkerService.Publisher;
using ESB.MassTransit.WorkerService.Publisher.Services;
using MassTransit;
using MassTransit.Transports;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost");
    });
});

builder.Services.AddHostedService<PublishMessageService>(provider =>
{
    using IServiceScope scope = provider.CreateScope();
    IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
    return new(publishEndpoint);
});



var host = builder.Build();
host.Run();
