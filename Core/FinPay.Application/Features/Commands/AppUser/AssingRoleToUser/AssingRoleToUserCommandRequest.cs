using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.AssingRoleToUser
{
    public class AssingRoleToUserCommandRequest : IRequest<AssingRoleToUserCommandRespose>
    {

        public string Id { get; set; }
        public string[] Roles { get; set; }
    }
}