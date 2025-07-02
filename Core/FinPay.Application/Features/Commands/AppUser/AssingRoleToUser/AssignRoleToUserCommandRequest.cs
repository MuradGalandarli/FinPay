using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.AssingRoleToUser
{
    public class AssignRoleToUserCommandRequest : IRequest<AssignRoleToUserCommandResponse>
    {

        public string Id { get; set; }
        public string[] Roles { get; set; }
    }
}