﻿using FinPay.Application.Service;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Domain.Entity.Paymet;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Repositoryes;
using Finpay.SignalR.ServiceHubs;
using AutoMapper;
using Microsoft.Extensions.Configuration;

public class RabbitMqListener : IRabbitMqListener
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ITransactionWriteRepository _transactionWriteRepository;
    private readonly ICardBalanceWriteRepository _cardBalanceWriteRepository;
    private readonly ICardBalanceReadRepository _cardBalanceReadRepository;
    private readonly ITransactionReadRepository _transactionReadRepository;
    private readonly IPaypalTransactionWriteRepository _paypalTransactionWriteRepository;
    private readonly ICardToCardServiceHub _cardToCardServiceHub;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public RabbitMqListener(ITransactionWriteRepository transactionWriteRepository, ICardBalanceWriteRepository cardBalanceWriteRepository, ICardBalanceReadRepository cardBalanceReadRepository, ITransactionReadRepository transactionReadRepository, IPaypalTransactionWriteRepository paypalTransactionWriteRepository, ICardToCardServiceHub cardToCardServiceHub, IMapper mapper, IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMq:Host"],
            Port = int.Parse(_configuration["RabbitMq:Port"]),
            UserName = _configuration["RabbitMq:UserName"],
            Password = _configuration["RabbitMq:Password"]
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _transactionWriteRepository = transactionWriteRepository;
        _cardBalanceWriteRepository = cardBalanceWriteRepository;
        _cardBalanceReadRepository = cardBalanceReadRepository;
        _transactionReadRepository = transactionReadRepository;
        _paypalTransactionWriteRepository = paypalTransactionWriteRepository;
        _cardToCardServiceHub = cardToCardServiceHub;
        _mapper = mapper;
        
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

                Retry(async () =>
                {
                    await DataHandler(message);
                    await onMessage(message);
                }).GetAwaiter().GetResult();


                Console.WriteLine($"[✔] Received message from '{queueName}': {json}");



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

    private async Task Retry(Func<Task> operation, int maxRetry = 3, int delayMs = 1000)
    {
        for (int attempt = 1; attempt <= maxRetry; attempt++)
        {
            try
            {
                await operation();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Retry cəhdi {attempt}/{maxRetry} - Xəta: {ex.Message}");

                if (attempt == maxRetry)
                {
                    Console.WriteLine("Retry limiti aşdı. Əməliyyat uğursuz oldu.");
                    throw;
                }

                await Task.Delay(delayMs);
            }
        }
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
                    await CreatePaymentOperation(createPaymentMQ);
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
    private async Task CreatePaymentOperation(CreatePaymentMQ createPaymentMQ)
    {
        var dateTime = await StringConverDateTimeUtc(createPaymentMQ.CreateAt.ToString());
        AppTransaction appTransaction = _mapper.Map<AppTransaction>(createPaymentMQ);
        
        await _transactionWriteRepository.Add(appTransaction);
        await _transactionWriteRepository.SaveAsync();
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
        if (cardToCardMQ != null)
        {
            string card = JsonConvert.SerializeObject(cardToCardMQ);
            var fromBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == cardToCardMQ.FromPaypalEmail);
            var toBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == cardToCardMQ.ToPaypalEmail);

            if (fromBalance == null || fromBalance.Balance < cardToCardMQ.Amount)
            {
                cardToCardMQ.Status = FinPay.Domain.Enum.CardToCardStatus.Failed;
                await PaypalTransactionAdd(cardToCardMQ);
                await _cardToCardServiceHub.AddedMessageCardToCard(cardToCardMQ.FromUserId, card);
                await _cardToCardServiceHub.AddedMessageCardToCard(cardToCardMQ.ToUserId, card);
                return false;
            }

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

            cardToCardMQ.Status = FinPay.Domain.Enum.CardToCardStatus.Completed;
            await PaypalTransactionAdd(cardToCardMQ);

            
            await _cardToCardServiceHub.AddedMessageCardToCard(cardToCardMQ.FromUserId, card);
            await _cardToCardServiceHub.AddedMessageCardToCard(cardToCardMQ.ToUserId, card);
            return true;

        }
        return false;
    }
    private async Task PaypalTransactionAdd(CardToCardMQ cardToCardMQ)
    {
        DateTime dateTime = await StringConverDateTimeUtc(cardToCardMQ.TransactionDate.ToString());

        PaypalTransaction transaction = _mapper.Map<PaypalTransaction>(cardToCardMQ);
      
        await _paypalTransactionWriteRepository.Add(transaction);
        await _cardBalanceWriteRepository.SaveAsync();
        await _transactionWriteRepository.SaveAsync();
    }

}
