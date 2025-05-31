using MediatR;

namespace FinPay.Application.Features.Commands.AppRole.CreateRole
{
    public class CreateRoleCommandRequest:IRequest<CreateRoleCommandResponse>
    {
       public string Name { get; set; }
    }
}