using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.DTOs
{
    public class GetUserBalanceResponse
    {
        public decimal Balance { get; set; }
        public string PaypalEmail { get; set; }
        //public string UserId { get; set; }
        public int UserAccountId { get; set; }
    }
}
