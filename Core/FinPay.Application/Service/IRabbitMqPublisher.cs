using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IRabbitMqPublisher
    {
        Task Publish<T>(string exchangeName, string routingKey, T message);
    }
}
