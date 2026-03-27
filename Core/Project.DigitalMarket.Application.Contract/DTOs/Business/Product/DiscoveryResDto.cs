namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryResDto
    {
        public List<DailyDiscoverItemDto> Items { get; set; } = new();
        public int Total { get; set; }
        public string ReqId { get; set; } = string.Empty;
    }

    public class DailyDiscoverItemDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid ThumbnailFileId { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public int SoldCount { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
        public Guid SellerId { get; set; }
        public string ShopName { get; set; } = string.Empty;
    }
}
