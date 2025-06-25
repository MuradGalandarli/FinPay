using MediatR;

namespace FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction
{
    public class CardTransactionCommandRequest : IRequest<CardTransactionCommandResponse>
    {
        public string? FromPaypalEmail { get; set; }
        public string? ToPaypalEmail { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}