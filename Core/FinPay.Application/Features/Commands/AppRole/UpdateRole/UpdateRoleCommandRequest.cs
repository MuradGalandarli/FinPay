using MediatR;
using System.Globalization;

namespace FinPay.Application.Features.Commands.AppRole.UpdateRole
{
    public class UpdateRoleCommandRequest:IRequest<UpdateRoleCOmmandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}