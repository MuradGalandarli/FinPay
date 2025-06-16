using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.MessageRetryEngine.Services
{
    public class TransactionMessageRabbitMq : ITransactionMessageRabbitMq
    {
        private readonly IModel _channel;

        public TransactionMessageRabbitMq()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(
                queue: "transaction-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public async Task ProcessAsync(TransactionMessage message)
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

            Console.WriteLine($"[>] Message sent: {message.Content}");

            await Task.CompletedTask;
        }

        public Task StartListeningAsync()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageContent = Encoding.UTF8.GetString(body);
                var message = new TransactionMessage { Content = messageContent };

                try
                {
                    await HandleMessageWithRetryAsync(message);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Processing error: {ex.Message}");

                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(
                queue: "transaction-queue",
                autoAck: false,
                consumer: consumer
            );

            Console.WriteLine("Waiting for RabbitMQ messages...");
            return Task.CompletedTask;
        }

        private async Task HandleMessageWithRetryAsync(TransactionMessage message)
        {
            int maxRetries = 3;
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"[i] Processing attempt {attempt}: {message.Content}");

                    await Task.Delay(100);

                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error during processing: {ex.Message}");

                    if (attempt == maxRetries)
                    {
                        Console.WriteLine("[!] Maximum retry attempts reached, message processing failed.");
                        throw;
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
            }
        }
    }
}
