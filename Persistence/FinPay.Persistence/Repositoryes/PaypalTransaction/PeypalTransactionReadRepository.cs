using FinPay.Application.Repositoryes.PaypalTransaction;
using FinPay.Domain.Entity.Paymet;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes
{
    public class PeypalTransactionReadRepository : ReadRepository<Domain.Entity.Paymet.PaypalTransaction>, IPaypalTransactionReadRepository
    {
        public PeypalTransactionReadRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
