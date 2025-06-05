using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity
{
    public class Menu:BaseEntity
    {
       
        public string Name { get; set; }
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
