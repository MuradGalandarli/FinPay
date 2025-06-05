using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IRoleService
    {
        Task<IDictionary<string, string>> GetAllRols(int page, int size);
        Task<(string id,string name)> GetRoleById(string id);
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string name);
        Task<bool> UpdateRole(string id, string name);
    }
}
