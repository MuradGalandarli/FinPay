using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.UserAccount
{
    public class UserAccountWriteRepository : WriteRepository<Domain.Entity.Paymet.UserAccount>, IUserAccountWriteRepository
    {
        public UserAccountWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
