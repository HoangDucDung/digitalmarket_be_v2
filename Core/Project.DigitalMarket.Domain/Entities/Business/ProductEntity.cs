using Project.DigitalMarket.Domain.Entities.Base;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    public static class ProductStatus
    {
        public const string Draft = "Draft";
        public const string Active = "Active";
        public const string Hidden = "Hidden";
        public const string Banned = "Banned";
    }

    /// <summary>
    /// Sản phẩm được hiển thị trên luồng khám phá.
    /// </summary>
    public class ProductEntity : BaseEntity
    {
        public Guid SellerId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public string ShopLocation { get; set; } = "Ho Chi Minh";
        public string Currency { get; set; } = "VND";
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public int DiscountPercent { get; set; }
        public int SoldCount { get; set; }
        public decimal RatingAverage { get; set; }
        public string CategoryBundle { get; set; } = "daily_discover_main";
        public string Status { get; set; } = ProductStatus.Active;
        public DateTime? PublishedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
    }
}
