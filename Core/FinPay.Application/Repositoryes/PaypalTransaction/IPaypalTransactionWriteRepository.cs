using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Repositoryes
{
    public interface IPaypalTransactionWriteRepository:IWriteRepository<Domain.Entity.Paymet.PaypalTransaction>
    {
    }
}
