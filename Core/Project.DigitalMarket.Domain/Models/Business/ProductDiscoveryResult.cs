namespace Project.DigitalMarket.Domain.Models.Business
{
    public class ProductDiscoveryResult
    {
        public List<ProductDiscoveryItem> Items { get; set; } = [];
        public int Total { get; set; }
    }

    public class ProductDiscoveryItem
    {
        public Guid ItemId { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public string ShopLocation { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int SoldCount { get; set; }
        public decimal RatingAverage { get; set; }
        public bool IsFeatured { get; set; }
    }
}
