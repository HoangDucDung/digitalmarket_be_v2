using Project.DigitalMarket.Domain.Entities.Base;
using Project.DigitalMarket.Domain.Share.Constants.Business;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    public class ProductEntity : BaseEntity
    {
        public Guid SellerId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Material { get; set; }
        public string Currency { get; set; } = "VND";
        public string Status { get; set; } = ProductConstants.Status.Draft;
        public DateTime? PublishedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public byte[] RowVersion { get; set; } = [];

        public virtual CategoryEntity? Category { get; set; }
        public virtual BrandEntity? Brand { get; set; }
        public virtual UserEntity Seller { get; set; } = null!;
        public virtual ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public virtual ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
        public virtual ProductRatingEntity? Rating { get; set; }
    }

    public class ProductImageEntity : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid FileId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
        public virtual ProductEntity Product { get; set; } = null!;
        public virtual FileEntity? File { get; set; }
    }

    public class ProductVariantEntity : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string? Sku { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ProductEntity Product { get; set; } = null!;
        public virtual ICollection<ProductVariantAttributeEntity> Attributes { get; set; } = new List<ProductVariantAttributeEntity>();
        public virtual ICollection<ProductInventoryMovementEntity> InventoryMovements { get; set; } = new List<ProductInventoryMovementEntity>();
    }

    public class ProductVariantAttributeEntity : BaseEntity
    {
        public Guid VariantId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
        public byte AttributeOrder { get; set; }
        public virtual ProductVariantEntity Variant { get; set; } = null!;
    }

    public class ProductInventoryMovementEntity : BaseEntity
    {
        public Guid VariantId { get; set; }
        public string ChangeType { get; set; } = "Initial";
        public int QuantityDelta { get; set; }
        public string? Note { get; set; }
        public virtual ProductVariantEntity Variant { get; set; } = null!;
    }

    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public Guid? ParentId { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual CategoryEntity? Parent { get; set; }
        public virtual ICollection<CategoryEntity> Children { get; set; } = new List<CategoryEntity>();
    }

    public class BrandEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class ProductRatingEntity
    {
        public Guid ProductId { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
        public virtual ProductEntity Product { get; set; } = null!;
    }
}
