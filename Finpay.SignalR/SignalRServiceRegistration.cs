using Finpay.SignalR.ServiceHubs;
using FinPay.Application.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.SignalR
{
    public static class SignalRServiceRegistration
    {
       public static void AddSignalRService(this IServiceCollection service)
        {
            service.AddScoped<ICardToCardServiceHub, CardToCardServiceHub>();
            service.AddScoped<ITransactionServiceHub, TransactionServiceHub>();
            service.AddSignalR();
        }
    }
}
