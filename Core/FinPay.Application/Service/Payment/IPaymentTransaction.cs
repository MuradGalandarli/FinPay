using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service.Payment
{
    public interface IPaymentTransaction
    {
        public Task<string> CreatePayment(decimal amount,string userId);
        public Task<string> CaptureOrderAsync(string orderId,string userId);
    }
}
