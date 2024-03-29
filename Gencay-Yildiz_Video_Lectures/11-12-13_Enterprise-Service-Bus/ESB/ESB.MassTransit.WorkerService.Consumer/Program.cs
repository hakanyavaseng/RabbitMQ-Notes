using ESB.MassTransit.WorkerService.Consumer;
using ESB.MassTransit.WorkerService.Consumer.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ExampleMessageConsumer>(); 
    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost");

        cfg.ReceiveEndpoint("example-queue", endpoint =>
        {
            endpoint.ConfigureConsumer<ExampleMessageConsumer>(context);
        });
    });

});

var host = builder.Build();
await host.RunAsync();
