using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface ICardToCardServiceHub
    {
        public Task AddedMessageCardToCard(string userId, string message);
    }
}
