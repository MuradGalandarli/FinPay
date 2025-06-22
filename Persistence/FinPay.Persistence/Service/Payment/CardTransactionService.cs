using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Repositoryes.PaypalTransaction;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinPay.Persistence.Service.Payment
{

    public class CardTransactionService: ICardTransactionService
    {
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly IPaypalTransactionReadRepository _paypalTransactionReadRepository;

        public CardTransactionService(IRabbitMqPublisher rabbitMqPublisher, IPaypalTransactionReadRepository paypalTransactionReadRepository)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _paypalTransactionReadRepository = paypalTransactionReadRepository;
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

            await _rabbitMqPublisher.Publish("transaction-exchange", "CardToCardKey", new CardToCardMQ
            {
                FromPaypalEmail = request.FromPaypalEmail,
                ToPaypalEmail = request.ToPaypalEmail,
                Amount = request.Amount,
                Description = request.Description,
                IsSuccessful = true,
                TransactionDate = DateTime.UtcNow,
                Status = Domain.Enum.CardToCardStatus.Pending
                
            });

            return true;
        }

        public async Task<bool> UpdateTransactionStatusAndPublish(int transactionId)
        {
            var paypalTransaction =await _paypalTransactionReadRepository.GetSingelAsync(x => x.IsSuccessful && x.Status == Domain.Enum.CardToCardStatus.Failed);
            if (paypalTransaction != null)
            {
                await _rabbitMqPublisher.Publish("transaction-exchange", "CardToCardKey", new CardToCardMQ
                {
                    FromPaypalEmail = paypalTransaction.FromPaypalEmail,
                    ToPaypalEmail = paypalTransaction.ToPaypalEmail,
                    Amount = paypalTransaction.Amount,
                    Description = paypalTransaction.Description,
                    IsSuccessful = true,
                    TransactionDate = DateTime.UtcNow,
                    Status = Domain.Enum.CardToCardStatus.Pending

                });

                return true;
            }
            return false;

        }
    }
}
