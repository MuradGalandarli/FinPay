using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application
{
    public static class ApplicationServiceRegistration
    {
       public static void AddApplicationService(this IServiceCollection services)
        {

            services.AddMediatR(typeof(ApplicationServiceRegistration));
          
        }
    }
}
