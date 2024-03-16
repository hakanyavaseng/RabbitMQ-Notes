
using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


#region Direct Exchange 
/*
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while(true)
{
    Console.Write("Message: ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "direct-queue-example", body: byteMessage);
}
*/
#endregion

#region Fanout Exchange
/*
channel.ExchangeDeclare("fanout-exchange-example", ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i + 1}");

    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        routingKey: string.Empty,
        body: message);
}
*/
#endregion

#region Topic Exchange
/*
channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i+1}");
    Console.Write("Please write topic: ");
    string topic = Console.ReadLine();

    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message);
    
}
*/
#endregion

#region Header Exchange
/* 
  x-match: İlgili queue'nun mesajı hangi davranışla alacağı kararını veren bir keydir.
   any => İlgili queue'nun sadece tek bir key-value değerinin eşleşmesi neticesinde mesajı alacağını ifade eder.
   all => Tüm key-value değerlerinin eşleşmesi neticesinde mesajı alacağını ifade eder. 
*/

channel.ExchangeDeclare("header-exchange-example", ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i + 1}");
    Console.Write("Please enter header value: ");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>()
    {
        ["no"] = value
    };

    channel.BasicPublish(
        exchange: "header-exchange-example", 
        routingKey: string.Empty, 
        body: message,
        basicProperties: basicProperties
        );
    
}


#endregion


Console.Read();