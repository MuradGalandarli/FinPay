using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IAuthorizationEndpointService
    {
        public Task AssingRoleEndpointAsync(string[] role,string menu, string code,Type type);
        public Task<List<string>> GetRolesToEndpointAsync(string code, string menu);
    }
}
