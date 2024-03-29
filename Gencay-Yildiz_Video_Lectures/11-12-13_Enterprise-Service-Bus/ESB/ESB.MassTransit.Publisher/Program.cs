using ESB.MassTransit.Shared.Messages;
using MassTransit;

string rabbitMqUri = "amqp://guest:guest@localhost";
string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMqUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMqUri}/{queueName}"));

Console.Write("Sent Message: ");
string message = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(message))
{
    await sendEndpoint.Send<IMessage>(new ExampleMessage()
    {
        Text = message
    });
}

Console.Read();
