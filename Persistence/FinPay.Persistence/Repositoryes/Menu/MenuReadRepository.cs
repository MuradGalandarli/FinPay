using FinPay.Application.Repositoryes.Menu;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.Menu
{
    public class MenuReadRepository : ReadRepository<Domain.Entity.Menu>, IMenuReadRepository
    {
        public MenuReadRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
