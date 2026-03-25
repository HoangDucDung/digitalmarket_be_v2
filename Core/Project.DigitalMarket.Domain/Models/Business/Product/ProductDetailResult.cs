namespace Project.DigitalMarket.Domain.Models.Business.Product
{
    public class ProductDetailResult
    {
        public Guid ItemId { get; set; }
        public Guid ShopId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Currency { get; set; } = "VND";
        public decimal Price { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public int DiscountPercent { get; set; }
        public decimal RatingStar { get; set; }
        public int SoldCount { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string ShopLocation { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
