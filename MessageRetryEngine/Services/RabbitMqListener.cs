using FinPay.Application.Service;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Domain.Entity.Paymet;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Repositoryes;

public class RabbitMqListener : IRabbitMqListener
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ITransactionWriteRepository _transactionWriteRepository;
    private readonly ICardBalanceWriteRepository _cardBalanceWriteRepository;
    private readonly ICardBalanceReadRepository _cardBalanceReadRepository;
    private readonly ITransactionReadRepository _transactionReadRepository;
    private readonly IPaypalTransactionWriteRepository _paypalTransactionWriteRepository;

    public RabbitMqListener(ITransactionWriteRepository transactionWriteRepository, ICardBalanceWriteRepository cardBalanceWriteRepository, ICardBalanceReadRepository cardBalanceReadRepository, ITransactionReadRepository transactionReadRepository, IPaypalTransactionWriteRepository paypalTransactionWriteRepository)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _transactionWriteRepository = transactionWriteRepository;
        _cardBalanceWriteRepository = cardBalanceWriteRepository;
        _cardBalanceReadRepository = cardBalanceReadRepository;
        _transactionReadRepository = transactionReadRepository;
        _paypalTransactionWriteRepository = paypalTransactionWriteRepository;
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
                   var dateTime = await StringConverDateTimeUtc(createPaymentMQ.CreateAt.ToString());
                    await _transactionWriteRepository.Add(new()
                    {
                        Amount = createPaymentMQ.Amount,
                        CreateAt = dateTime,
                        UserAccountId = createPaymentMQ.UserAccountId,
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
                CardToCardMQ? cardToCardMQ = datas as CardToCardMQ;
                await CardToCardOperation(cardToCardMQ);
                break;
        }
    }

    private async Task<DateTime> StringConverDateTimeUtc(string date)
    {
        if (DateTime.TryParse(date, out DateTime result))
        {
            return DateTime.SpecifyKind(result, DateTimeKind.Local).ToUniversalTime();
        }
        return DateTime.MinValue;
    }

    private async Task<bool> CardToCardOperation(CardToCardMQ cardToCardMQ)
    {
        if(cardToCardMQ != null)
        {
            var fromBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == cardToCardMQ.FromPaypalEmail);
            var toBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == cardToCardMQ.ToPaypalEmail);

            if (fromBalance == null || fromBalance.Balance < cardToCardMQ.Amount)
                return false;
              
            if (toBalance == null)
            {
                var userAccountId = await _transactionReadRepository.GetSingelAsync(x => x.PaypalEmail == cardToCardMQ.ToPaypalEmail);
                toBalance = new CardBalance
                {
                    PaypalEmail = cardToCardMQ.ToPaypalEmail,
                    Balance = 0
                };

                await _cardBalanceWriteRepository.Add(toBalance);
            }

            fromBalance.Balance -= cardToCardMQ.Amount;
            toBalance.Balance += cardToCardMQ.Amount;

            DateTime dateTime = await StringConverDateTimeUtc(cardToCardMQ.TransactionDate.ToString());
            var transaction = new PaypalTransaction
            {
                FromPaypalEmail = cardToCardMQ.FromPaypalEmail,
                ToPaypalEmail = cardToCardMQ.ToPaypalEmail,
                Amount = cardToCardMQ.Amount,
                Description = cardToCardMQ.Description,
                IsSuccessful = cardToCardMQ.IsSuccessful,
                TransactionDate = dateTime

            };

            await _paypalTransactionWriteRepository.Add(transaction);
            await _cardBalanceWriteRepository.SaveAsync();
            await _transactionWriteRepository.SaveAsync();
            return true;

        }
        return false;
    }
}
