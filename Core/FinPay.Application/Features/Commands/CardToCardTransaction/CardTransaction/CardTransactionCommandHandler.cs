using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction
{
    public class CardTransactionCommandHandler : IRequestHandler<CardTransactionCommandRequest, CardTransactionCommandResponse>
    {
        private readonly ICardTransactionService _cardTransactionService;

        public CardTransactionCommandHandler(ICardTransactionService cardTransactionService)
        {
            _cardTransactionService = cardTransactionService;
        }

        public async Task<CardTransactionCommandResponse> Handle(CardTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            var cardTransactionCommandResponse = await _cardTransactionService.PaypalToPaypalAsync(new()
            {
                Amount = request.Amount,
                Description = request.Description,
                FromPaypalEmail = request.FromPaypalEmail,
                ToPaypalEmail = request.ToPaypalEmail
            });
            return new() { Status = cardTransactionCommandResponse };

        }
    }
}
