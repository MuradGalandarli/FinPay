using MediatR;

namespace FinPay.Application.Features.Commands.Payment.CaptureOrder
{
    public class CaptureOrderCommandRequest:IRequest<CaptureOrderCommandResponse>
    {
        public string Id { get; set; }
    }
}