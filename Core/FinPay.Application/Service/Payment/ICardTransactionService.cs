using FinPay.Application.DTOs.CardTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service.Payment
{
    public interface ICardTransactionService
    {
        public Task<bool> PaypalToPaypalAsync(CardToCardRequestDto request);
        Task<bool> UpdateTransactionStatusAndPublish(int transactionId);
    }
}
