
namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryResDto
    {
        public List<FeedItemDto> Feeds { get; set; }
        public int FeedTotal { get; set; }
        public string ReqId { get; set; } // Dùng để trace log hệ thống
    }
}
