using System;
using System.ComponentModel.DataAnnotations;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class ProductCreateReqDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string CategoryId { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Description { get; set; }
        [MinLength(1)]
        public List<Guid> Images { get; set; } = new();
        public string? Material { get; set; }
        public string? Sku { get; set; }
        public string Status { get; set; } = "draft";
        public bool EnableVariation { get; set; }
        public string? VariationName { get; set; }
        public List<ProductVariantCreateReqDto> Variations { get; set; } = new();
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
    }

    public class ProductVariantCreateReqDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(typeof(decimal), "0", "999999999999")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public string? Sku { get; set; }
    }

    public class ProductUpdateReqDto
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
    }

    public class ProductCreateResDto
    {
        public Guid ProductId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ReqId { get; set; } = Guid.NewGuid().ToString("N");
    }
}
