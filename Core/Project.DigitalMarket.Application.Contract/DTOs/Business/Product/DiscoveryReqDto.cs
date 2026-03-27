using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryReqDto
    {
        public int Limit { get; set; } = 60;
        public int Offset { get; set; } = 0;
    }
}
