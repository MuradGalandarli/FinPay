using FinPay.Application.RabbitMqMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface ITransactionMessageRabbitMq
    {
        Task ProcessAsync<T>(string routingKey, T message);
        Task StartListeningAsync<T>(string routingKey,string queue);
    }
}
