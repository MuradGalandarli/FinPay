using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Domain.Entity
{
    public class Tokeninfo:BaseEntity
    {
        

        [Required]
        [MaxLength(30)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiredAt { get; set; }
    }
}
