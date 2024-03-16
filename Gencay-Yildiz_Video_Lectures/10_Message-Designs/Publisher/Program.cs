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

byte[] message = Encoding.UTF8.GetBytes("Hello!");

channel.BasicPublish(
    exchange: "",
    routingKey: queueName,
    body: message);
*/
#endregion

#region Publish / Subscribe (Pub/Sub) Tasarımı
/*
string exchangeName = "example-pubsub-exchange";

channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Hello! {i}");

    channel.BasicPublish(
    exchange: exchangeName,
    routingKey: "",
    body: message);

}
*/
#endregion

#region Work-Queue Tasarımı
//Bu tasarımda publisher tarafından yayınlanmış bir mesajın birden fazla consumer arasından yalnızca biri tarafından alınması sağlanır. 
//Böylece mesajların işlenmesi sürecinde tüm consumer'lar aynı iş yüküne ve eşit görev dağılımına sahip olur.
//Genellikle direct-exchange kullanılarak tasarlanır.
/*
string queueName = "example-work-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(1000);
    byte[] message = Encoding.UTF8.GetBytes($"Hello! {i}");
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: queueName,
        basicProperties: properties,
        body: message);    
}
*/
#endregion

#region Request/Response Tasarımı
//Bu tasarımda publisher bir request yapar gibi kuyruğa mesaj gönderir ve bu mesajı tüketen consumer'dan sonuca dair başka bir kuyruğa mesaj göndermesini bekler.

string requestQueueName = "example-request-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string responseQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Request Mesajını Oluşturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = responseQueueName;

for (int i = 0; i < 10; i++)
{
    Task.Delay(100).Wait();    
    byte[] message = Encoding.UTF8.GetBytes("Hello" + (i + 1));
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        basicProperties: properties,
        body: message);
}
#endregion


#region Response Kuyruğu Dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: responseQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine("Response: " + message);
    }
};

#endregion

#endregion

Console.Read();