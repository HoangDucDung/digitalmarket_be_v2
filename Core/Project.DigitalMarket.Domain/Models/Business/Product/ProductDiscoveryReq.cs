namespace Project.DigitalMarket.Domain.Models.Business.Product
{
    public class ProductDiscoveryReq
    {
        public int Limit { get; set; } = 40;
        public int Offset { get; set; } = 0;
        public string? Bundle { get; set; } = "daily_discover_main";
        public string? Keyword { get; set; }
    }

    public class ProductDiscoveryResult
    {
        public List<ProductDiscoveryItem> Items { get; set; } = new();
        public int Total { get; set; }
    }

    public class ProductDiscoveryItem
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid ThumbnailFileId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public int SoldCount { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
    }
}
