
using FinPay.Application.Service;
using FinPay.SignalR;
using FinPay.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finpay.SignalR.ServiceHubs
{
    public class TransactionServiceHub : ITransactionServiceHub
    {
        private readonly IHubContext<TransactionHub> _hubContext;

        public TransactionServiceHub(IHubContext<TransactionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task TransactionAddedMessage(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync(ReceiceFunctionNames.TransactionAddedMessage, message);
        }
    }
}
