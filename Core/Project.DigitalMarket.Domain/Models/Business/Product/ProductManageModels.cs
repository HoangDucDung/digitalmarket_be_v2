using System;

namespace Project.DigitalMarket.Domain.Models.Business.Product
{
    public class ProductCreateReq
    {
        public Guid SellerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public List<Guid> Images { get; set; } = new();
        public string? Material { get; set; }
        public string? Sku { get; set; }
        public string Status { get; set; } = "draft";
        public bool EnableVariation { get; set; }
        public string? VariationName { get; set; }
        public List<ProductVariantCreateReq> Variations { get; set; } = new();
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class ProductVariantCreateReq
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }
    }

    public class ProductUpdateReq
    {
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }

        public string? Name { get; set; }
        public string? Status { get; set; }
    }

    public class CategoryTreeReq
    {
        public bool IncludeDisabled { get; set; }
    }

    public class CategoryNodeResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public int Level { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public int? SortOrder { get; set; }
        public List<CategoryNodeResult>? Children { get; set; }
    }
}
