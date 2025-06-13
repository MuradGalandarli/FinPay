using FinPay.Domain.Entity.Paymet;
using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.DTOs
{
    public class UserAccountDto
    {
        public string UserId { get; set; }
        public string Currency { get; set; } = "AZN";
        public string? LinkedPaypalEmail { get; set; }
       
    }
}
