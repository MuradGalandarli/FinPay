using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Service;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FinPay.MessageRetryEngine.Services
{
    internal class RabbitMqListenerService : BackgroundService
    {
        private readonly ITransactionMessageRabbitMq _transactionMessageRabbitMq;

        public RabbitMqListenerService(ITransactionMessageRabbitMq transactionMessageRabbitMq)
        {
            _transactionMessageRabbitMq = transactionMessageRabbitMq;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _transactionMessageRabbitMq.StartListeningAsync<CardToCardMQ>("CardToCardPayment", "CardToCard");
            await _transactionMessageRabbitMq.StartListeningAsync<CreatePaymentMQ>("CreatePayment", "transaction");


            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
