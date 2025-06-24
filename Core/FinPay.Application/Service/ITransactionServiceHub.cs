using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface ITransactionServiceHub
    {
        public Task TransactionAddedMessage(string userId, string message);
    }
}
