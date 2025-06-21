using FinPay.Domain.Enum;
using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.RabbitMqMessage
{
    public class CardToCardMQ
    {
        public string FromPaypalEmail { get; set; }
        public string ToPaypalEmail { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UserId { get; set; }
        public CardToCardStatus Status { get; set; }
     
    }
}
