using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point to Point) Tasarımı
/*
string queueName = "example-p2p-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
*/
#endregion

#region Publish / Subscribe (Pub/Sub) Tasarımı
/*
string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "example-pubsub-exchange",
    routingKey: "");

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
       queue: queueName,
       autoAck: true,
       consumer: consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
*/
#endregion


#region Work-Queue Tasarımı
/*
string queueName = "example-work-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

channel.BasicQos(
    prefetchCount: 1,
    prefetchSize: 0,
    global: false);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};   
*/
#endregion

#region Request / Response Tasarımı
string requestQueueName = "example-request-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(message);

    #region Response to Publisher
    byte[] responseMessage = Encoding.UTF8.GetBytes($"Task completed: " + message);

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: properties,
        body: responseMessage);
    #endregion
};
#endregion


Console.Read();