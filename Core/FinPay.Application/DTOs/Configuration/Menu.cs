using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.DTOs.Configuration
{
    public  class Menu
    {
        public string Name { get; set; }
        public List<Action> Action { get; set; } = new();
    }
}
