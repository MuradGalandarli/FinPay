
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Domain.Entity;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.AppTransactions
{
    public class TransactionReadRepository : ReadRepository<AppTransaction>, ITransactionReadRepository
    {
        public TransactionReadRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
