using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Service.Payment;
using FinPay.Domain.Entity.Paymet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service.Payment
{

    public class CardTransactionService: ICardTransactionService
    {
        private readonly ITransactionWriteRepository _transactionWriteRepository;
        private readonly ICardBalanceWriteRepository _cardBalanceWriteRepository;
        private readonly ICardBalanceReadRepository _cardBalanceReadRepository;
        private readonly IPaypalTransactionWriteRepository _paypalTransactionWriteRepository;
        

        public CardTransactionService(
            ICardBalanceWriteRepository cardBalanceWriteRepository,
            ITransactionWriteRepository transactionWriteRepository,
            ICardBalanceReadRepository cardBalanceReadRepository,
            IPaypalTransactionWriteRepository paypalTransactionWriteRepository)
        {
            _cardBalanceWriteRepository = cardBalanceWriteRepository;
            _transactionWriteRepository = transactionWriteRepository;
            _cardBalanceReadRepository = cardBalanceReadRepository;
            _paypalTransactionWriteRepository = paypalTransactionWriteRepository;
        }

        public async Task<bool> PaypalToPaypalAsync(CardToCardRequestDto request)
        {
            if (request.Amount <= 0 ||
                string.IsNullOrWhiteSpace(request.FromPaypalEmail) ||
                string.IsNullOrWhiteSpace(request.ToPaypalEmail) ||
                request.FromPaypalEmail == request.ToPaypalEmail)
            {
                return false;
            }

          
            var fromBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == request.FromPaypalEmail);
            var toBalance = await _cardBalanceReadRepository.GetSingelAsync(x => x.PaypalEmail == request.ToPaypalEmail);

            if (fromBalance == null || fromBalance.Balance < request.Amount)
                return false;

            if (toBalance == null)
            {
                toBalance = new CardBalance
                {
                    PaypalEmail = request.ToPaypalEmail,
                    Balance = 0
                };

                await _cardBalanceWriteRepository.Add(toBalance);
            }

            fromBalance.Balance -= request.Amount;
            toBalance.Balance += request.Amount;

           
            var transaction = new PaypalTransaction
            {
                FromPaypalEmail = request.FromPaypalEmail, 
                ToPaypalEmail = request.ToPaypalEmail,
                Amount = request.Amount,
                Description = request.Description,
                IsSuccessful = true,
                TransactionDate = DateTime.UtcNow
            };

            await _paypalTransactionWriteRepository.Add(transaction);
            await _cardBalanceWriteRepository.SaveAsync();
            await _transactionWriteRepository.SaveAsync();

            return true;
        }
    }
}
