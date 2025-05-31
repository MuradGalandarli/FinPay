using MediatR;

namespace FinPay.Application.Features.Commands.AppRole.DeleteRole
{
    public class DeleteRoleCommandRequest:IRequest<DeleteRoleCommandResponse>
    {
        public string Name { get; set; }
    }
}