using FinPay.Application.Enums;
using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity.Paymet
{
    public class UserAccount:BaseEntity
    {
        public string UserId { get; set; }  
        public string Currency { get; set; } = "AZN";
        public UserAccountStatus Status { get; set; } = UserAccountStatus.Active; // Active, Blocked, Closed
        public string? LinkedPaypalEmail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public CardBalance CardBalance { get; set; }
    }
}
