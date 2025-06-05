using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.Endpoint
{
    public class EndpointReadRepository:ReadRepository<Domain.Entity.Endpoint>,IEndpointReadRepository
    {
        private readonly AppDbContext _appDbContext;
        public EndpointReadRepository(AppDbContext appDbContext):base(appDbContext)
        {
            
        }

    }
}
