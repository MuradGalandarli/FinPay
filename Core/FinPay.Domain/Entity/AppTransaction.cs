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

namespace FinPay.Domain.Entity
{
    public class AppTransaction:BaseEntity
    {
        public string? FromUserId { get; set; }

        [ForeignKey(nameof(FromUserId))]
        public ApplicationUser? FromUser { get; set; }

        public string? ToUserId { get; set; }

        [ForeignKey(nameof(ToUserId))]
        public ApplicationUser? ToUser { get; set; }

        public decimal Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public TransferStatus Status { get; set; } = TransferStatus.Pending;

    }
}
