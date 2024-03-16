using RabbitMQ.Client;
using System.Text;

// Create a connection factory for RabbitMQ
ConnectionFactory factory = new();
factory.HostName = "localhost";

// Create a connection using the factory
using IConnection connection = factory.CreateConnection();

// Create a channel using the connection
using IModel channel = connection.CreateModel();

// Declare a queue named "example-queue" on the channel
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

// Create basic properties for messages
IBasicProperties properties = channel.CreateBasicProperties();

// Set the messages to be persistent, meaning they will survive broker restarts
properties.Persistent = true; // This ensures both the queue and published messages will be durable. However, this doesn't guarantee message won't be lost completely! (Outbox & Inbox design patterns are used for this)

// Publish 100 messages to the queue
for (int i = 0; i < 100; i++)
{
    // Simulate some delay
    Task.Delay(200).Wait();

    // Encode the message into a byte array
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i + 1}");

    // Publish the message to the queue
    channel.BasicPublish(
        exchange: "",
        routingKey: "example-queue",
        body: message,
        basicProperties: properties
    );
}

// Wait for input to keep the console window open
Console.Read();
