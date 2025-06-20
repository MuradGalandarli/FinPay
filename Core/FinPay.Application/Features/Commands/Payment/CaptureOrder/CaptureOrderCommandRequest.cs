using MediatR;

namespace FinPay.Application.Features.Commands.Payment.CaptureOrder
{
    public class CaptureOrderCommandRequest:IRequest<CaptureOrderCommandResponse>
    {
        public string OrderId { get; set; }
        public int UserAccountId { get; set; }
      
    }
}