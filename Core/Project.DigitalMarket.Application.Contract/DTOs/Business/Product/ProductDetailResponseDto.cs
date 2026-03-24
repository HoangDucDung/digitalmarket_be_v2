namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class ProductDetailResponseDto
    {
        public ProductItemDetailDto Item { get; set; } = new();
        public ProductPriceDetailDto ProductPrice { get; set; } = new();
        public ProductReviewDetailDto ProductReview { get; set; } = new();
        public ProductShopDetailDto ShopDetailed { get; set; } = new();
        public string ReqId { get; set; } = string.Empty;
    }

    public class ProductItemDetailDto
    {
        public Guid ItemId { get; set; }
        public Guid ShopId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Currency { get; set; } = "VND";
        public int ShowDiscount { get; set; }
        public decimal Price { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public decimal RatingStar { get; set; }
        public string ShopLocation { get; set; } = string.Empty;
        public int HistoricalSold { get; set; }
        public DateTime CTime { get; set; }
        public bool IsFreeShipping { get; set; } = true;
    }

    public class ProductPriceDetailDto
    {
        public int Discount { get; set; }
        public decimal Price { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public bool HidePrice { get; set; }
    }

    public class ProductReviewDetailDto
    {
        public decimal RatingStar { get; set; }
        public int TotalRatingCount { get; set; }
        public int CmtCount { get; set; }
        public int HistoricalSold { get; set; }
    }

    public class ProductShopDetailDto
    {
        public Guid ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShopLocation { get; set; } = string.Empty;
        public decimal RatingStar { get; set; }
    }
}
