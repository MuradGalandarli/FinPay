using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Application.Repositoryes.Menu;
using FinPay.Application.Service;
using FinPay.Application.Service.Configurations;
using FinPay.Domain.Entity;
using FinPay.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service.Authentications
{
    public class AuthorizetionEndpointService : IAuthorizationEndpointService
    {
        private readonly IApplicationService _applicationService;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IEndpointWriteRepository _endpointWriteRepository;
        private readonly IMenuWriteRepository _menuWriteRepository;
        private readonly IMenuReadRepository _menuReadRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AuthorizetionEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<ApplicationRole> roleManager)
        {
            _applicationService = applicationService;
            _endpointReadRepository = endpointReadRepository;
            _endpointWriteRepository = endpointWriteRepository;
            _menuReadRepository = menuReadRepository;
            _menuWriteRepository = menuWriteRepository;
            _roleManager = roleManager;
        }
        async Task IAuthorizationEndpointService.AssingRoleEndpointAsync(string[] role, string menu, string code, Type type)
        {
            Domain.Entity.Endpoint? endpoint = await _endpointReadRepository.Table.Include(x => x.Menu).FirstOrDefaultAsync(e => e.Menu.Name == menu && e.Code == code);

            Domain.Entity.Menu _menu = await _menuReadRepository.GetSingelAsync(m => m.Name == menu);
            if (_menu == null)
            {
                await _menuWriteRepository.Add(new() { Name = menu });
                await _endpointWriteRepository.SaveAsync();
            }
            else
            {
                if (endpoint == null)
                {
                    var action = _applicationService.GetAuthorizeDefinitionEndponit(type)
                    .FirstOrDefault(m => m.Name == menu)?.Action.FirstOrDefault(c => c.Code == code);

                    endpoint = new()
                    {
                        Code = action.Code,
                        ActionType = action.ActionType,
                        HttpType = action.HttpType,
                        Definition = action.Definition,
                        Menu = _menu
                    };

                    await _endpointWriteRepository.Add(endpoint);
                    await _endpointWriteRepository.SaveAsync();

                }
            }

            var appRoles = await _roleManager.Roles.Where(r => role.Contains(r.Name)).ToListAsync();
            foreach (var rol in endpoint.ApplicationRoles.ToList())
                endpoint.ApplicationRoles.Remove(rol);

            var appRole = await _roleManager.Roles.Where(r => role.Contains(r.Name)).ToListAsync();

            foreach (var rol in appRoles)
                endpoint.ApplicationRoles.Add(rol);

            await _endpointWriteRepository.SaveAsync();
        }

        public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
        {
            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.ApplicationRoles)
                .Include(e => e.Menu)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
            if (endpoint != null)
                return endpoint.ApplicationRoles.Select(r => r.Name).ToList();
            return null;
        }
    }
}
