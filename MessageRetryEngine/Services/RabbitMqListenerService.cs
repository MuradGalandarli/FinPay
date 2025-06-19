using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Service;

public class RabbitMqListenerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqListenerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var listener = scope.ServiceProvider.GetRequiredService<IRabbitMqListener>();

        await listener.StartListening<CardToCardMQ>(
            exchangeName: "transaction-exchange",
            queueName: "CardToCard",
            routingKey: "CardToCardKey",
            async (message) =>
            {
                Console.WriteLine($"[>] Received from CardToCard: {JsonConvert.SerializeObject(message)}");
                await Task.CompletedTask;
            });

        await listener.StartListening<CreatePaymentMQ>(
            exchangeName: "transaction-exchange",
            queueName: "transaction",
            routingKey: "transactionKey",
            async (message) =>
            {
                Console.WriteLine($"[>] Received from transaction: {JsonConvert.SerializeObject(message)}");
                await Task.CompletedTask;
            });

        Console.WriteLine("Listener is active. Waiting...");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
