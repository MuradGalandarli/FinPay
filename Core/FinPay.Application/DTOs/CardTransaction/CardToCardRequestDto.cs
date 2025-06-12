using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.DTOs.CardTransaction
{
    public class CardToCardRequestDto
    {
        public string FromPaypalEmail { get; set; }   
        public string ToPaypalEmail { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
