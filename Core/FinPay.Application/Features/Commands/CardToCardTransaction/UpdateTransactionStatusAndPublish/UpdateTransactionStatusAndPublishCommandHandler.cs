using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.CardToCardTransaction.UpdateTransactionStatusAndPublish
{
    public class UpdateTransactionStatusAndPublishCommandHandler : IRequestHandler<UpdateTransactionStatusAndPublishCommandRequest, UpdateTransactionStatusAndPublishCommandResponse>
    {
        private readonly ICardTransactionService _cardTransactionService;

        public UpdateTransactionStatusAndPublishCommandHandler(ICardTransactionService cardTransactionService)
        {
            _cardTransactionService = cardTransactionService;
        }

        public async Task<UpdateTransactionStatusAndPublishCommandResponse> Handle(UpdateTransactionStatusAndPublishCommandRequest request, CancellationToken cancellationToken)
        {
            bool updateTransactionStatus = await _cardTransactionService.UpdateTransactionStatusAndPublish(request.TransactionId);
           
            return new()
            {
                Status = updateTransactionStatus,
            };
        }
    }
}
