using FinPay.Domain.Entity.Paymet;
using FinPay.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinPay.Application.Features.Queries.Transaction.PaymrntTransaction
{
    public class PaymrntTransactionQueryRespose
    {
        public int UserAccountId { get; set; }
        public bool IsPayoutSent { get; set; } = false ;
        public string? PaypalEmail { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public TransferStatus Status { get; set; } = TransferStatus.Pending;
    }
}