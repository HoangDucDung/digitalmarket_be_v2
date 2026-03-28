using Microsoft.EntityFrameworkCore;
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
    public class ProductManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IProductManager
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

        public async Task<ProductDiscoveryResult> GetDailyDiscoverAsync(ProductDiscoveryReq request)
        {
            var limit = Math.Clamp(request.Limit, 1, 100);
            var offset = Math.Max(0, request.Offset);
            var query = _productRepository.GetDiscoverQuery();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.PublishedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .Select(x => new ProductDiscoveryItem
                {
                    ProductId = x.Id,
                    SellerId = x.SellerId,
                    Name = x.Name,
                    Slug = x.Slug,
                    ThumbnailFileId = x.Images
                        .OrderByDescending(i => i.IsPrimary)
                        .ThenBy(i => i.SortOrder)
                        .Select(i => i.FileId)
                        .FirstOrDefault(),
                    Price = x.Variants
                        .Where(v => v.IsActive)
                        .Min(v => (decimal?)v.Price) ?? 0,
                    OriginalPrice = x.Variants.Where(v => v.IsActive).Select(v => v.OriginalPrice).FirstOrDefault(),
                    DiscountPercent = 0,
                    SoldCount = 0,
                    AvgRating = x.Rating != null ? x.Rating.AvgRating : 0,
                    RatingCount = x.Rating != null ? x.Rating.RatingCount : 0,
                    ShopName = x.Seller.FullName ?? string.Empty
                })
                .ToListAsync();

            return new ProductDiscoveryResult { Items = items, Total = total };
        }

        public async Task<ProductDetailResult?> GetProductDetailAsync(ProductDetailReq request)
        {
            return await _productRepository.GetDiscoverQuery()
                .Where(x => x.Id == request.ItemId)
                .Select(x => new ProductDetailResult
                {
                    ProductId = x.Id,
                    SellerId = x.SellerId,
                    Name = x.Name,
                    Slug = x.Slug,
                    Description = x.Description,
                    Material = x.Material,
                    Currency = x.Currency,
                    Status = x.Status,
                    CategoryName = x.Category != null ? x.Category.Name : null,
                    BrandName = x.Brand != null ? x.Brand.Name : null,
                    EnableVariation = x.Variants.Count > 1 || x.Variants.Any(v => v.VariantName != "Default"),
                    Images = x.Images.OrderBy(i => i.SortOrder).Select(i => new ProductDetailImageResult
                    {
                        FileId = i.FileId,
                        SortOrder = i.SortOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList(),
                    Variants = x.Variants.Where(v => v.IsActive).Select(v => new ProductDetailVariantResult
                    {
                        VariantId = v.Id,
                        VariantName = v.VariantName,
                        Sku = v.Sku,
                        Price = v.Price,
                        OriginalPrice = v.OriginalPrice,
                        StockQuantity = v.StockQuantity,
                        Attributes = v.Attributes.OrderBy(a => a.AttributeOrder).Select(a => new ProductDetailVariantAttributeResult
                        {
                            AttributeName = a.AttributeName,
                            AttributeValue = a.AttributeValue,
                            AttributeOrder = a.AttributeOrder
                        }).ToList()
                    }).ToList(),
                    //MinPrice = x.Variants.Where(v => v.IsActive).Select(v => v.Price).DefaultIfEmpty(0).Min(),
                    //MaxPrice = x.Variants.Where(v => v.IsActive).Select(v => v.Price).DefaultIfEmpty(0).Max(),
                    SoldCount = 0,
                    ShopName = x.Seller.FullName ?? string.Empty,
                    AvgRating = x.Rating != null ? x.Rating.AvgRating : 0,
                    RatingCount = x.Rating != null ? x.Rating.RatingCount : 0
                })
                .FirstOrDefaultAsync();
        }

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
            while (await _productRepository.GetByCondition(x => x.Slug == slug).AnyAsync())
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

        public async Task<bool> UpdateProductAsync(ProductUpdateReq request)
        {
            if (request.SellerId == Guid.Empty) throw new BusinessException(ErrorCode.InvalidProductData, "SellerId không hợp lệ.");
            if (request.ProductId == Guid.Empty) throw new BusinessException(ErrorCode.InvalidProductData, "ProductId không hợp lệ.");

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null || product.IsDeleted || product.SellerId != request.SellerId)
                throw new BusinessException(ErrorCode.ProductNotFound, "Sản phẩm không tồn tại.");

            if (!string.IsNullOrWhiteSpace(request.Name)) product.Name = request.Name.Trim();
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                var nextStatus = request.Status.Trim();
                if (!IsValidStatus(nextStatus)) throw new BusinessException(ErrorCode.InvalidProductData, "Status không hợp lệ.");
                product.Status = nextStatus;
            }

            product.UpdatedAt = GenerateExtentions.Now;
            product.UpdatedBy = request.SellerId.ToString();
            product.PublishedAt = string.Equals(product.Status, ProductConstants.Status.Published, StringComparison.OrdinalIgnoreCase)
                ? product.PublishedAt ?? GenerateExtentions.Now
                : null;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid sellerId, Guid productId)
        {
            if (sellerId == Guid.Empty) throw new BusinessException(ErrorCode.InvalidProductData, "SellerId không hợp lệ.");
            if (productId == Guid.Empty) throw new BusinessException(ErrorCode.InvalidProductData, "ProductId không hợp lệ.");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.IsDeleted || product.SellerId != sellerId)
                throw new BusinessException(ErrorCode.ProductNotFound, "Sản phẩm không tồn tại.");

            product.IsDeleted = true;
            product.IsActive = false;
            product.Status = ProductConstants.Status.Archived;
            product.PublishedAt = null;
            product.UpdatedAt = GenerateExtentions.Now;
            product.UpdatedBy = sellerId.ToString();

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteProductByItemIdAsync(Guid sellerId, Guid itemId)
        {
            return DeleteProductAsync(sellerId, itemId);
        }

        public async Task<List<CategoryNodeResult>> GetCategoryTreeAsync(CategoryTreeReq request)
        {
            var categories = await _productRepository.GetCategoryTreeQuery(request.IncludeDisabled)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var nodes = categories.ToDictionary(
                x => x.Id,
                x => new CategoryNodeResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    Level = x.Level,
                    ParentId = x.ParentId,
                    SortOrder = x.SortOrder,
                    Children = new List<CategoryNodeResult>()
                });

            foreach (var node in nodes.Values)
            {
                if (node.ParentId.HasValue && nodes.TryGetValue(node.ParentId.Value, out var parent))
                {
                    parent.Children!.Add(node);
                }
            }

            foreach (var node in nodes.Values)
            {
                if (node.Children is { Count: > 0 })
                {
                    node.Children = node.Children
                        .OrderBy(x => x.SortOrder ?? int.MaxValue)
                        .ThenBy(x => x.Name)
                        .ToList();
                }

                node.IsLeaf = node.Children is null || node.Children.Count == 0;
                if (node.Children is { Count: 0 })
                {
                    node.Children = null;
                }
            }

            return nodes.Values
                .Where(x => !x.ParentId.HasValue || !nodes.ContainsKey(x.ParentId.Value))
                .OrderBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.Name)
                .ToList();
        }
    }
}
