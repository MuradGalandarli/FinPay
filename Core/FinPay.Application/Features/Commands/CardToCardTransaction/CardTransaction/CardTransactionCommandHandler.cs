using AutoMapper;
using FinPay.Application.DTOs.CardTransaction;
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
        private readonly IMapper _mapper;

        public CardTransactionCommandHandler(ICardTransactionService cardTransactionService, IMapper mapper)
        {
            _cardTransactionService = cardTransactionService;
            _mapper = mapper;
        }

        public async Task<CardTransactionCommandResponse> Handle(CardTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CardToCardRequestDto cardToCardRequestDto = _mapper.Map<CardToCardRequestDto>(request);

            var cardTransactionCommandResponse = await _cardTransactionService.PaypalToPaypalAsync(cardToCardRequestDto);
           
            return new() { Status = cardTransactionCommandResponse };

        }
    }
}
