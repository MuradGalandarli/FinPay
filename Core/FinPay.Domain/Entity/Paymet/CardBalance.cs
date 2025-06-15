using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity.Paymet
{
    public class CardBalance:BaseEntity
    {
        public decimal Balance { get; set; }
        public string PaypalEmail { get; set; }
        public bool IsActive { get; set; }
        
        //public string UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        //public ApplicationUser ApplicationUser { get; set; }

        public int UserAccountId { get; set; }
        [ForeignKey(nameof(UserAccountId))]
        public UserAccount UserAccount { get; set; }

    }
}
