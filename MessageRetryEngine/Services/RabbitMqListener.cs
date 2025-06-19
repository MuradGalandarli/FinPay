using FinPay.Application.Service;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Domain.Enum;
using FinPay.Domain.Entity.Paymet;
using System.Data;

public class RabbitMqListener : IRabbitMqListener
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ITransactionWriteRepository _transactionWriteRepository;

    public RabbitMqListener(ITransactionWriteRepository transactionWriteRepository)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _transactionWriteRepository = transactionWriteRepository;
    }

    public Task StartListening<T>(string exchangeName, string queueName, string routingKey, Func<T, Task> onMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);

        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, exchangeName, routingKey);

        consumer.Received += (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<T>(json);

                DataHandler(message).GetAwaiter().GetResult();


                Console.WriteLine($"[✔] Received message from '{queueName}': {json}");

                onMessage(message).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✘] Error processing message: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        Console.WriteLine($"[*] Listening on queue '{queueName}' with routing key '{routingKey}'");

        return Task.CompletedTask;
    }

    private async Task DataHandler<T>(T datas)
    {
        var type = datas.GetType().Name;
        switch (type)
        {


            case "CreatePaymentMQ":
                CreatePaymentMQ? createPaymentMQ = datas as CreatePaymentMQ;

                try
                {

                    await _transactionWriteRepository.Add(new()
                    {
                        Amount = createPaymentMQ.Amount,
                        CreateAt = DateTime.UtcNow,
                        //CreateAt = createPaymentMQ.CreateAt,
                        FromUserId = createPaymentMQ.FromUserId,
                        PaypalEmail = createPaymentMQ.PaypalEmail,
                        IsPayoutSent = createPaymentMQ.IsPayoutSent,
                        Status = createPaymentMQ.Status,
                       
                        

                    });
                    await _transactionWriteRepository.SaveAsync();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
                break;

            case "CardToCard":


                break;
        }
    }

}
