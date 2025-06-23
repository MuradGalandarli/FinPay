using FinPay._Infrastructure.Service.Configurations;
using FinPay.Application.Service.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay._Infrastructure
{
    public static class InfrastructureServiceRegistraction
    {
        public static void AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<IApplicationService, ApplicationService>();
          
        }
    }
}
