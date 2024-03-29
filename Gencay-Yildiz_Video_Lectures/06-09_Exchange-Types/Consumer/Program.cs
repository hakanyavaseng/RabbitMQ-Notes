﻿
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


#region Direct Exchange 
/*
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};
*/
#endregion

#region FanoutExchange
/*
channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

Console.WriteLine("Enter queue name: ");
string? _queueName = Console.ReadLine();

channel.QueueDeclare(
    queue: _queueName,
    exclusive: false);

channel.QueueBind(
    queue: _queueName,
    exchange: "fanout-exchange-example",
    routingKey: string.Empty
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: _queueName,
    autoAck: true,
    consumer : consumer
    );

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
*/
#endregion


#region Topic Exchange
/*
channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );


Console.Write("Please write topic to listen: ");
string topic = Console.ReadLine();


string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queueName, "topic-exchange-example", topic);


EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
*/

#endregion


#region Header Exchange
channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers
    );

Console.Write("Please enter header value: ");
string value = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "header-exchange-example",
    routingKey: string.Empty,
    arguments: new Dictionary<string, object>
    {
        ["x-match"] = "all",
        ["no"] = value

    });

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};


#endregion




Console.Read();