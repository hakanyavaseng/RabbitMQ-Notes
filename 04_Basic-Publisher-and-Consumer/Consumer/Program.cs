using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

/* Order of Operations for Consumer Application */

// Creating Connection 
ConnectionFactory factory = new();
factory.HostName = "localhost";

// Activating Connection and Opening Channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Creating Queue
channel.QueueDeclare(queue: "example-queue", exclusive: false); // The queue in the consumer should be configured exactly the same as in the publisher.

// Reading Messages from Queue
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: "example-queue",
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    // Place where the received message from the queue is processed.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

};

Console.Read();
