using FinPay.Application.Service;
using FinPay.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
         IdentityResult result = await _roleManager.CreateAsync(new() {Id = Guid.NewGuid().ToString(), Name = name});
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                return false; 

            IdentityResult result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IDictionary<string, string>> GetAllRols(int page,int size)
        {
            var datas = await _roleManager.Roles.Skip(page*size).Take(size).ToDictionaryAsync(role => role.Id, role => role.Name);
            return datas;
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            return (role.Id, role.Name);
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new() { Id = id, Name = name});
            return result.Succeeded;
        }
    }
}
