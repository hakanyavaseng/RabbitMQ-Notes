using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create a connection factory for RabbitMQ
ConnectionFactory factory = new();
factory.HostName = "localhost";

// Create a connection using the factory
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Declare a queue named "example-queue" on the channel
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

// Create a consumer for handling incoming messages
EventingBasicConsumer consumer = new(channel);

// Start consuming messages from the queue
channel.BasicConsume(
    queue: "example-queue",
    autoAck: false, // For using BasicAck, managing Acknowledgement manually
    consumer: consumer
);

#region Fair Despatch
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
/*
    prefetchSize: Determines the maximum message size (in bytes) that a consumer can receive. 0 means unlimited.
    prefetchCount: Determines the number of messages that a consumer can process at the same time.
    global: Specifies whether this configuration applies to all consumers or only to the consumer being called.
*/
#endregion

// Event handler for received messages
consumer.Received += async (sender, e) =>
{
    // Print the received message
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    // Simulate some processing time
    await Task.Delay(1000);

    bool result = true;

    // Depending on the result of processing, acknowledge or reject the message
    if (result)
        #region Basic Ack
        // If autoAck is false and there is no feedback to RabbitMQ, messages will be requeued after 30 minutes.
        channel.BasicAck(e.DeliveryTag, multiple: false); // The Delivery Tag uniquely identifies a message. 'multiple' indicates if it applies to a single message or multiple.
    #endregion

    else
        #region Basic Nack
        // Reject the message and requeue it
        channel.BasicNack(e.DeliveryTag, multiple: false, requeue: true); // 'requeue' parameter determines whether to requeue the message.
    #endregion

    #region Basic Cancel
    // Reject all messages from the queue
    channel.BasicCancel(e.ConsumerTag);
    #endregion

    #region Basic Reject
    // Reject a specific message
    channel.BasicReject(deliveryTag: 3, requeue: true);
    #endregion
};

// Wait for input to keep the console window open
Console.Read();
