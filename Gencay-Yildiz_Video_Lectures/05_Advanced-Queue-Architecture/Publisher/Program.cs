using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection(); 
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true; // Hem kuyruğun hem de kuyrukta yayınlanmış mesajların kalıcı olmasını sağlar. Ancak bu iletinin kaybolmayacağını tam olarak garanti etmez! (Outbox & Inbox design pattern'lar bunun için kullanılır)

for (int i = 0; i < 100; i++)
{
    Task.Delay(200).Wait();
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i + 1}");
    channel.BasicPublish(
        exchange: "", 
        routingKey: "example-queue", 
        body: message,
        basicProperties: properties
        );
}

Console.Read();
