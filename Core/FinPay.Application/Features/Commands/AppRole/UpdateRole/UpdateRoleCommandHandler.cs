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
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<UpdateRoleCommandRequest> _validator;

        public UpdateRoleCommandHandler(IRoleService roleService, IValidator<UpdateRoleCommandRequest> validator)
        {
            _roleService = roleService;
            _validator = validator;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            throw new Exceptions.ValidationException(validationResult);

            bool status = await _roleService.UpdateRole(request.Id, request.Name);
            return new()
            {
                Status = status,
            };
        }
    }
}
