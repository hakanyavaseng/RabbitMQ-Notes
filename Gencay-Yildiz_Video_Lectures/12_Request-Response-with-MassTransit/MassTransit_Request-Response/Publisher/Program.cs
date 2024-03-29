
using MassTransit;
using Shared.RequestResponseMessage;

string rabbitMQUri = "amqp://guest:guest@localhost";
string requestQueue = "request-queue";
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

await bus.StartAsync();

IRequestClient<RequestMessage> request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));

int i = 1;
while(true)
{
    var response = await request.GetResponse<ResponseMessage>(new()
    {
         MessageNo = i, Text = $"Request: {i++}"
    });
    Console.WriteLine($"Response received : {response.Message.Text}");
}

