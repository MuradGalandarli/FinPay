using AutoMapper;
using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Repositoryes.PaypalTransaction;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using FinPay.Domain.Entity.Paymet;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinPay.Persistence.Service.Payment
{

    public class CardTransactionService : ICardTransactionService
    {
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly IPaypalTransactionReadRepository _paypalTransactionReadRepository;
        private readonly IMapper _mapper;

        public CardTransactionService(IRabbitMqPublisher rabbitMqPublisher, IPaypalTransactionReadRepository paypalTransactionReadRepository, IMapper mapper)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _paypalTransactionReadRepository = paypalTransactionReadRepository;
            _mapper = mapper;
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
            CardToCardMQ cardToCardMQ = _mapper.Map<CardToCardMQ>(request);
            await _rabbitMqPublisher.Publish("transaction-exchange", "CardToCardKey", cardToCardMQ);
            
            return true;
        }

        public async Task<bool> UpdateTransactionStatusAndPublish(int transactionId)
        {
            var paypalTransaction = await _paypalTransactionReadRepository.GetSingelAsync(x => x.IsSuccessful && x.Status == Domain.Enum.CardToCardStatus.Failed);
            if (paypalTransaction != null)
            {
                CardToCardMQ cardToCardMQ = _mapper.Map<CardToCardMQ>(paypalTransaction);
                await _rabbitMqPublisher.Publish("transaction-exchange", "CardToCardKey",cardToCardMQ);

                return true;
            }
            return false;

        }
    }
}
