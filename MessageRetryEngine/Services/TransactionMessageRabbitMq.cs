using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Channels;
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

        public async Task ProcessAsync<T>(string routingKey, T message)
        {
            _channel.ExchangeDeclare(exchange: "transaction-exchange", type: ExchangeType.Direct);

            _channel.QueueDeclare(queue: "transaction", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(queue: "CardToCard", durable: true, exclusive: false, autoDelete: false);
            
             _channel.QueueBind(queue: "transaction", exchange: "transaction-exchange", routingKey: routingKey);
             _channel.QueueBind(queue: "CardToCard", exchange: "transaction-exchange", routingKey: routingKey);
           
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "transaction-exchange",
                routingKey: routingKey,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[>] Message sent: {json}");

            await Task.CompletedTask;
        }

        public Task StartListeningAsync<T>(string routingKey, string queue)
        {
            _channel.ExchangeDeclare(exchange: "transaction-exchange", type: ExchangeType.Direct);

            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
           
            _channel.QueueBind(queue: queue, exchange: "transaction-exchange", routingKey: routingKey);
            _channel.QueueBind(queue: queue, exchange: "transaction-exchange", routingKey: routingKey);
          
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageContent = Encoding.UTF8.GetString(body);
                CardToCardMQ transactionMessage = JsonConvert.DeserializeObject<CardToCardMQ>(messageContent);
                var message = JsonConvert.DeserializeObject<T>(messageContent);

               
                try
                {
                    await HandleMessageWithRetryAsync(transactionMessage);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Processing error: {ex.Message}");

                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

                _channel.BasicConsume(
                    queue: queue,
                    autoAck: false,
                    consumer: consumer
            );

            Console.WriteLine("Waiting for RabbitMQ messages...");
            return Task.CompletedTask;
        }

        private async Task HandleMessageWithRetryAsync(CardToCardMQ message)
        {
            int maxRetries = 3;
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"[i] Processing attempt {attempt}: {message}");

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
