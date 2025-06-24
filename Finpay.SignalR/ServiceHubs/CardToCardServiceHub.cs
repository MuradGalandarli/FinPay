using Finpay.SignalR.Hubs;
using FinPay.Application.Service;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finpay.SignalR.ServiceHubs
{
    public class CardToCardServiceHub : ICardToCardServiceHub
    {
        private readonly IHubContext<CardToCardHub> _hubContext;

        public CardToCardServiceHub(IHubContext<CardToCardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AddedMessageCardToCard(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync(ReceiceFunctionNames.CardToCardAddedMessage,message);
        }
    }
}
