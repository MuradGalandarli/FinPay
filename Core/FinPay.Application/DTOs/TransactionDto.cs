using FinPay.Domain.Entity.Paymet;
using FinPay.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.DTOs
{
    public class TransactionDto
    {
        public int UserAccountId { get; set; }
        public bool IsPayoutSent { get; set; } = false;
        public string? PaypalEmail { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public TransferStatus Status { get; set; }
    }
}
