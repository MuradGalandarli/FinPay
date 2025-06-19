using FinPay.Application.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IConnection _connection;

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
    }

    public async Task Publish<T>(string exchangeName, string routingKey, T message)
    {
        using var channel = _connection.CreateModel();

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body
        );

        Console.WriteLine($"[x] Published: {json}");
    }
}
