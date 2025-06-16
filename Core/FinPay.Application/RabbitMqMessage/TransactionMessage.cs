using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.RabbitMqMessage
{
    public class TransactionMessage
    {
        public string Message { get; set; }
        public string Content { get; set; }
    }
}
