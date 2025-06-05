using FinPay.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity
{
    public class Endpoint:BaseEntity
    {
        public Endpoint()
        {
            ApplicationRoles = new HashSet<ApplicationRole>();
        }
        public string? ActionType { get; set; }
        public string? HttpType { get; set; }
        public string? Definition { get; set; }
        public string? Code { get; set; }
        public Menu? Menu { get; set; }
        public ICollection<ApplicationRole> ApplicationRoles { get; set; }


    }
}
