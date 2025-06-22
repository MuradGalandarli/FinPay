using MediatR;

namespace FinPay.Application.Features.Commands.CardToCardTransaction.UpdateTransactionStatusAndPublish
{
    public class UpdateTransactionStatusAndPublishCommandRequest:IRequest<UpdateTransactionStatusAndPublishCommandResponse>
    {
        public int TransactionId { get; set; }
    }
}