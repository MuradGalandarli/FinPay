using FinPay.Domain.Entity.Paymet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Repositoryes.AppTransactions
{
    public interface ITransactionWriteRepository:IWriteRepository<AppTransaction>
    {
    }
}
