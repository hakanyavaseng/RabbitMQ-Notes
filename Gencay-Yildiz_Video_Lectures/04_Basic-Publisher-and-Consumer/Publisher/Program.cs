using RabbitMQ.Client;
using System.Text;
/* Order of Operations for Publisher Application */

// Creating Connection
ConnectionFactory factory = new();
factory.HostName = "localhost";
//factory.Uri = new("amqps://..."); => If working on the Cloud.

// Activating Connection and Opening Channel
using IConnection connection = factory.CreateConnection(); // Using 'using' to optimize memory since IConnection is IDisposable.
using IModel channel = connection.CreateModel();

// Creating Queue
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// Sending Messages to Queue

// RabbitMQ accepts messages to be sent to the queue as bytes, so messages need to be converted to bytes.

//byte[] message = Encoding.UTF8.GetBytes("Hello!");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message); // Default exchange applied (direct exchange)

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i + 1}");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}

Console.Read();
