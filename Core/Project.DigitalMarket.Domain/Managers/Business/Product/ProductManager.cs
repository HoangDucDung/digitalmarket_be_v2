using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Models.Business.Product;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.Extensions.Extensions;
using System.Text.RegularExpressions;

namespace Project.DigitalMarket.Domain.Managers.Business.Product
{
    internal sealed class ProductManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IProductManager
    {
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        private static string Slugify(string value)
        {
            var slug = value.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", string.Empty);
            return slug.Trim('-');
        }

        private static bool IsValidStatus(string status) =>
            string.Equals(status, ProductConstants.Status.Draft, StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, ProductConstants.Status.Published, StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, ProductConstants.Status.Archived, StringComparison.OrdinalIgnoreCase);

        public async Task<Guid> AddProductAsync(ProductCreateReq request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) throw new BusinessException(ErrorCode.InvalidProductData, "Tên sản phẩm không hợp lệ.");
            if (string.IsNullOrWhiteSpace(request.Category)) throw new BusinessException(ErrorCode.InvalidProductData, "Category không hợp lệ.");
            if (request.Images.Count == 0) throw new BusinessException(ErrorCode.InvalidProductData, "Sản phẩm phải có ít nhất 1 ảnh.");

            if (!request.EnableVariation)
            {
                if (request.Price is null || request.Stock is null) throw new BusinessException(ErrorCode.InvalidProductData, "Thiếu price/stock cho sản phẩm không biến thể.");
                if (request.Price < 0 || request.Stock < 0) throw new BusinessException(ErrorCode.InvalidProductData, "price/stock không hợp lệ.");
            }
            else
            {
                if (request.Variations.Count == 0) throw new BusinessException(ErrorCode.InvalidProductData, "Phải có ít nhất 1 variant.");
                if (request.Variations.Any(v => v.Price < 0 || v.Stock < 0)) throw new BusinessException(ErrorCode.InvalidProductData, "Giá hoặc tồn kho của variant không hợp lệ.");
            }

            var status = request.Status.Trim().Equals("published", StringComparison.OrdinalIgnoreCase)
                ? ProductConstants.Status.Published
                : ProductConstants.Status.Draft;
            if (!IsValidStatus(status)) throw new BusinessException(ErrorCode.InvalidProductData, "Status không hợp lệ.");

            var now = GenerateExtentions.Now;
            var baseSlug = Slugify(request.Name);
            var slug = baseSlug;
            var suffix = 1;
            while (await _productRepository.IsSlugExistsAsync(slug))
            {
                slug = $"{baseSlug}-{suffix++}";
            }

            var category = await _productRepository.ResolveCategoryAsync(request.Category.Trim());
            var brand = await _productRepository.ResolveBrandAsync(request.Brand?.Trim());

            var product = new ProductEntity
            {
                SellerId = request.SellerId,
                CategoryId = category?.Id,
                BrandId = brand?.Id,
                Name = request.Name.Trim(),
                Slug = slug,
                Description = request.Description,
                Material = request.Material,
                Currency = "VND",
                Status = status,
                PublishedAt = status == ProductConstants.Status.Published ? now : null,
                IsActive = request.IsActive,
                CreatedAt = now,
                CreatedBy = request.SellerId.ToString()
            };

            for (var i = 0; i < request.Images.Count; i++)
            {
                product.Images.Add(new ProductImageEntity
                {
                    FileId = request.Images[i],
                    SortOrder = i,
                    IsPrimary = i == 0
                });
            }

            if (!request.EnableVariation)
            {
                product.Variants.Add(new ProductVariantEntity
                {
                    VariantName = "Default",
                    Price = request.Price!.Value,
                    StockQuantity = request.Stock!.Value,
                    Sku = request.Sku,
                    CreatedAt = now
                });
            }
            else
            {
                var tiers = (request.VariationName ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (tiers.Length > 2) throw new BusinessException(ErrorCode.InvalidProductData, "Chỉ hỗ trợ tối đa 2 tier.");
                foreach (var v in request.Variations)
                {
                    var variant = new ProductVariantEntity
                    {
                        VariantName = v.Name.Trim(),
                        Price = v.Price,
                        StockQuantity = v.Stock,
                        Sku = v.Sku,
                        CreatedAt = now
                    };

                    var values = v.Name.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    for (byte order = 1; order <= Math.Min(2, values.Length); order++)
                    {
                        variant.Attributes.Add(new ProductVariantAttributeEntity
                        {
                            AttributeOrder = order,
                            AttributeName = tiers.Length >= order ? tiers[order - 1] : $"Tier {order}",
                            AttributeValue = values[order - 1]
                        });
                    }

                    product.Variants.Add(variant);
                }
            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return product.Id;
        }
    }
}
