using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppRole.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCOmmandResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<UpdateRoleCommandRequest> _validator;

        public UpdateRoleCommandHandler(IRoleService roleService, IValidator<UpdateRoleCommandRequest> validator)
        {
            _roleService = roleService;
            _validator = validator;
        }

        public async Task<UpdateRoleCOmmandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            if (_validator.Validate(request).IsValid)
            {
                bool statsu = await _roleService.UpdateRole(request.Id, request.Name);
                return new()
                {
                    Status = statsu,
                };
            }
            throw new Exceptions.ValidationException();
        }
    }
}
