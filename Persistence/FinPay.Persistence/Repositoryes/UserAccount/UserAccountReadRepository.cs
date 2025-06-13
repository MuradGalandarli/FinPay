using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.UserAccount
{
    public class UserAccountReadRepository : ReadRepository<Domain.Entity.Paymet.UserAccount>, IUserAccountReadRepository
    {
        public UserAccountReadRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
