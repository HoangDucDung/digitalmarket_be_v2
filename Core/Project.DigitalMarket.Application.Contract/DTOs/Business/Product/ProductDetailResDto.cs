namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class ProductDetailResDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Material { get; set; }
        public string Currency { get; set; } = "VND";
        public string Status { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public List<ProductDetailImageDto> Images { get; set; } = new();
        public bool EnableVariation { get; set; }
        public List<ProductDetailVariantDto> Variants { get; set; } = new();
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int SoldCount { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
        public Guid SellerId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string ReqId { get; set; } = string.Empty;
    }

    public class ProductDetailImageDto
    {
        public Guid FileId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ProductDetailVariantDto
    {
        public Guid VariantId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public string? Sku { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int StockQuantity { get; set; }
        public List<ProductDetailVariantAttributeDto> Attributes { get; set; } = new();
    }

    public class ProductDetailVariantAttributeDto
    {
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
        public byte AttributeOrder { get; set; }
    }
}
