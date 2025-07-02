using AutoMapper;
using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction
{
    public class CardTransactionCommandHandler : IRequestHandler<CardTransactionCommandRequest, CardTransactionCommandResponse>
    {
        private readonly ICardTransactionService _cardTransactionService;
        private readonly IMapper _mapper;
        private readonly IValidator<CardTransactionCommandRequest> _validator;
        private readonly IMetricsService _metricsService;

        public CardTransactionCommandHandler(ICardTransactionService cardTransactionService, IMapper mapper, IValidator<CardTransactionCommandRequest> validator, IMetricsService metricsService)
        {
            _cardTransactionService = cardTransactionService;
            _mapper = mapper;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<CardTransactionCommandResponse> Handle(CardTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("PaypalToPaypal");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                CardToCardRequestDto cardToCardRequestDto = _mapper.Map<CardToCardRequestDto>(request);
                var cardTransactionCommandResponse = await _cardTransactionService.PaypalToPaypalAsync(cardToCardRequestDto);

                return new() { Status = cardTransactionCommandResponse };
            }
            finally
            {
                sw.Stop();
                _metricsService.ObserveHistogram("PaypalToPaypal_duration_seconds", sw.Elapsed.TotalSeconds);
            }
           
           
        }
    }
}
