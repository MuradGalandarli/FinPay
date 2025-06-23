using FinPay.Application.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.MessageRetryEngine
{
    public static class RabbitMQServiceRegistration
    {
        public static void AddRabbitMQService(this IServiceCollection services)
        {
            services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            services.AddScoped<IRabbitMqListener, RabbitMqListener>();
            services.AddHostedService<RabbitMqListenerService>();
        }
    }
}
