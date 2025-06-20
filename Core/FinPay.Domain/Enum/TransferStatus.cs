using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Enum
{
    public enum TransferStatus
    {
        Pending,    // Ödəniş başladıldı, lakin tamamlanmayıb
        Completed,  // Ödəniş uğurla tamamlandı
        Failed,     // Ödəniş uğursuz oldu
        Canceled,   // İstifadəçi ödənişi ləğv etdi
        Refunded,   // Pul geri qaytarıldı
        Created,
        Success
    }
}
