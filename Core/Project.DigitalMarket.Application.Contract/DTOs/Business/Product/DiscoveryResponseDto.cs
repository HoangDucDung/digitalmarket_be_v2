using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryResponseDto
    {
        public List<FeedItemDto> Feeds { get; set; }
        public int FeedTotal { get; set; }
        public string ReqId { get; set; } // Dùng để trace log hệ thống
    }
}
