using FinPay.Domain.Entity.Paymet;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Identity
{
    public class ApplicationUser:IdentityUser
{
    public string Name { get; set; } = string.Empty;
        public ICollection<AppTransaction> SendTransactions { get; set; }
        public ICollection<AppTransaction> ReceivedTransactions { get; set; }
        public ICollection<PaypalTransaction> PaypalTransactions { get; set; }
        //public CardBalance cardBalance { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
