using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;


namespace FinPay.Persistence.Service.Payment
{

    public class CardTransactionService: ICardTransactionService
    {
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public CardTransactionService(IRabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
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
            });

            return true;
        }
    }
}
