using FinPay.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Identity
{
    public class ApplicationRole:IdentityRole<string>
    {

        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
