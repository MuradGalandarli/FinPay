using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Domain.Entity.Paymet;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes
{
    public class CardBalanceWriteRepository : WriteRepository<CardBalance>, ICardBalanceWriteRepository
    {
        public CardBalanceWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
