using FinPay.Application.Service;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.AssingRoleToUser
{
    public class AssingRoleToUserCommandHandler : IRequestHandler<AssingRoleToUserCommandRequest, AssingRoleToUserCommandRespose>
    {
        private readonly IUserService _userService;

        public AssingRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssingRoleToUserCommandRespose> Handle(AssingRoleToUserCommandRequest request, CancellationToken cancellationToken)
        {
          await  _userService.AssingRoleToUser(request.Id,request.Roles);
            return new();
        }
    }
}
