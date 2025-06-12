using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.PaypalTransaction;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.PaypalTransaction
{
    public class PeypalTransactionWriteRepository : WriteRepository<Domain.Entity.Paymet.PaypalTransaction>, IPaypalTransactionWriteRepository
    {
        public PeypalTransactionWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
