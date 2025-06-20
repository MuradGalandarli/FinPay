using FinPay.Domain.Enum;
using FinPay.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity.Paymet
{
    public class AppTransaction : BaseEntity
    {
        public int UserAccountId { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public  UserAccount? UserAccount { get; set; }
        public bool IsPayoutSent { get; set; } = false;
        public string? PaypalEmail { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public TransferStatus Status { get; set; } = TransferStatus.Pending;

    }
}
