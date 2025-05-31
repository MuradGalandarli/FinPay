using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppRole.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCOmmandResponse>
    {
        private readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<UpdateRoleCOmmandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
          bool statsu = await _roleService.UpdateRole(request.Id,request.Name);
            return new()
            {
                Status = statsu,
            };
        }
    }
}
