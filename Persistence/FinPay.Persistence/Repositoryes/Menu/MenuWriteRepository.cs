using FinPay.Application.Repositoryes.Menu;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.Menu
{
    public class MenuWriteRepository : WriteRepository<Domain.Entity.Menu>, IMenuWriteRepository
    {
        public MenuWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
