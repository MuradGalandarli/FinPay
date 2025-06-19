using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IRabbitMqListener
    {
        Task StartListening<T>(string exchangeName, string queueName, string routingKey, Func<T, Task> onMessage);  
    }
}
