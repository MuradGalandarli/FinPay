using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes.Endpoint
{
    public class EndpointWriteRepository : WriteRepository<Domain.Entity.Endpoint>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
