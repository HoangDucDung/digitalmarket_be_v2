using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Domain.Models.Auths
{
    public class InfoToken
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
