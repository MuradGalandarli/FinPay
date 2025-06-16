using FinPay.Application.RabbitMqMessage;   
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.MessageRetryEngine.RabbitMqPublisher
{
    public class AppPublisher
    {
        private readonly IModel _channel;

        public AppPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare("transaction-queue", durable: true, exclusive: false, autoDelete: false);
        }

        public void Publish(TransactionMessage message)
        {
            var body = Encoding.UTF8.GetBytes(message.Content);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: "transaction-queue",
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[>] Send message: {message.Content}");
        }
    }
}

