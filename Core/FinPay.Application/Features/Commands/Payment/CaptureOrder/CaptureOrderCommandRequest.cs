using MediatR;

namespace FinPay.Application.Features.Commands.Payment.CaptureOrder
{
    public class CaptureOrderCommandRequest:IRequest<CaptureOrderCommandResponse>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int TransactionId { get; set; }
    }
}