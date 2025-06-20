using FinPay.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity.Paymet
{
    public class PaypalTransaction : BaseEntity
    {

        public string FromPaypalEmail { get; set; }
        public string ToPaypalEmail { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public bool IsSuccessful { get; set; }
        public DateTime TransactionDate { get; set; }
        public int UserAccountId { get; set; }
        [ForeignKey(nameof(UserAccountId))]
        public UserAccount UserAccount { get; set; }
    }
}

