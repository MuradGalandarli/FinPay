using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.AppRole.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRoleQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
           var data = await _roleService.GetAllRols();
            return new()
            {
              Roles = data,
            };
        }
    }
}
