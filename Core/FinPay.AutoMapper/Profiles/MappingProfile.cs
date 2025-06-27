using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.DTOs.CardTransaction;
using FinPay.Application.Features.Commands.AppUser.LoginUser;
using FinPay.Application.Features.Commands.AppUser.RefreshToken;
using FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction;
using FinPay.Application.Features.Queries.Transaction.PaymrntTransaction;
using FinPay.Application.RabbitMqMessage;
using FinPay.Domain.Entity.Paymet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.AutoMapper.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<TokenDto, RefreshTokenCommandResponse>();
            CreateMap<RefreshTokenCommandResponse, TokenDto>();

            CreateMap<TokenDto, LoginUserCommandResponse>();
            CreateMap<LoginUserCommandResponse, TokenDto>();

            CreateMap<PaymrntTransactionQueryRespose, TransactionDto>();
            CreateMap<TransactionDto,PaymrntTransactionQueryRespose>();

            CreateMap<CardToCardMQ, PaypalTransaction>();
            CreateMap<PaypalTransaction, CardToCardMQ>();

            CreateMap<CardToCardMQ, CardToCardRequestDto>();
            CreateMap<CardToCardRequestDto, CardToCardMQ>();

            CreateMap<CreatePaymentMQ, AppTransaction>();
            CreateMap<AppTransaction, CreatePaymentMQ>();

            CreateMap<CardToCardRequestDto, CardTransactionCommandRequest>();
            CreateMap<CardTransactionCommandRequest, CardToCardRequestDto>();

        }
    }
}
