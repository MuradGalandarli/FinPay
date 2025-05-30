using FinPay.Application.DTOs.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service.Configurations
{
    public interface IApplicationService
    {
      public List<Menu> GetAuthorizeDefinitionEndponit(Type type);
    }
}
