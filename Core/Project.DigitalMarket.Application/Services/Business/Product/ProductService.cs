using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Domain.Managers.Business.Product;
using Project.DigitalMarket.Domain.Models.Business.Product;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Application.Services.Business.Product
{
    internal sealed class ProductService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<ProductService>(lazyloadProvider), IProductService
    {
        private IProductManager _productManager => _lazyloadProvider.LazyGetRequiredService<IProductManager>();
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        public async Task<DiscoveryResDto> GetDailyDiscoverAsync(DiscoveryReqDto discoveryRequestDto)
        {
            var limit = Math.Clamp(discoveryRequestDto.Limit ?? 30, 1, 100);
            var offset = Math.Max(0, discoveryRequestDto.Offset ?? 0);
            
            var (items, total) = await _productRepository.GetPagedDiscoveryAsync(limit, offset, discoveryRequestDto.Keyword);

            var feeds = items.Select(p => new FeedItemDto
            {
                CentralisedItemCard = new CentralisedItemCardDto
                {
                    ItemData = new
                    {
                        Itemid = p.Id,
                        Shopid = p.SellerId,
                        Price = p.Variants.Where(v => v.IsActive).Min(v => (decimal?)v.Price) ?? 0,
                        OriginalPrice = p.Variants.Where(v => v.IsActive).Select(v => v.OriginalPrice).FirstOrDefault(),
                        Discount = 0
                    },
                    ItemCardDisplayedAsset = new
                    {
                        Name = p.Name,
                        ShopName = p.Seller.FullName ?? string.Empty,
                        SoldCountText = "0",
                        Rating = p.Rating?.RatingCount ?? 0,
                        Image = $"https://localhost:7097/api/files/{p.Images.OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder).Select(i => i.FileId).FirstOrDefault()}"
                    }
                }
            }).ToList();

            return new DiscoveryResDto
            {
                Feeds = feeds,
                FeedTotal = total,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<ProductDetailResDto?> GetProductDetailAsync(ProductDetailReqDto requestDto)
        {
            var p = await _productRepository.GetProductDetailByIdAsync(requestDto.ItemId);
            if (p is null) return null;

            return new ProductDetailResDto
            {
                Item = new ProductItemDetailDto
                {
                    ItemId = p.Id,
                    ShopId = p.SellerId,
                    Title = p.Name,
                    Image = p.Images.Any() ? $"https://localhost:7097/api/files/{p.Images.OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder).First().FileId}" : string.Empty,
                },
                ItemVariants = p.Variants.Where(v => v.IsActive).Select(v => new ProductDetailVariantDto
                {
                    VariantId = v.Id,
                    VariantName = v.VariantName,
                    Sku = v.Sku,
                    Price = v.Price,
                    OriginalPrice = v.OriginalPrice,
                    StockQuantity = v.StockQuantity,
                    Attributes = v.Attributes.OrderBy(a => a.AttributeOrder).Select(a => new ProductDetailVariantAttributeDto
                    {
                        AttributeName = a.AttributeName,
                        AttributeValue = a.AttributeValue,
                        AttributeOrder = a.AttributeOrder
                    }).ToList()
                }).ToList(),
                ProductReview = new ProductReviewDetailDto
                {
                    RatingStar = p.Rating?.AvgRating ?? 0,
                    TotalRatingCount = p.Rating?.RatingCount ?? 0,
                    CmtCount = 0,
                    HistoricalSold = 0
                },
                ShopDetailed = new ProductShopDetailDto
                {
                    ShopId = p.SellerId,
                    Name = p.Seller.FullName ?? string.Empty
                },
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<ProductCreateResDto> AddProductAsync(ProductCreateReqDto requestDto)
        {
            var productId = await _productManager.AddProductAsync(new ProductCreateReq
            {
                SellerId = UserId,
                Name = requestDto.Name,
                Category = requestDto.CategoryId,
                Brand = requestDto.Brand,
                Description = requestDto.Description,
                Images = requestDto.Images,
                Material = requestDto.Material,
                Sku = requestDto.Sku,
                Status = requestDto.Status,
                EnableVariation = requestDto.EnableVariation,
                VariationName = requestDto.VariationName,
                Variations = requestDto.Variations.Select(v => new ProductVariantCreateReq
                {
                    Name = v.Name,
                    Price = v.Price,
                    Stock = v.Stock,
                    Sku = v.Sku
                }).ToList(),
                Price = requestDto.Price,
                Stock = requestDto.Stock,
                IsActive = true
            });

            return new ProductCreateResDto
            {
                ProductId = productId,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateReqDto requestDto)
        {
            var product = await _productRepository.GetByIdAsync(requestDto.ProductId);
            if (product == null || product.IsDeleted || product.SellerId != UserId)
                throw new BusinessException(ErrorCode.ProductNotFound, "Sản phẩm không tồn tại.");

            if (!string.IsNullOrWhiteSpace(requestDto.Name)) product.Name = requestDto.Name.Trim();
            if (!string.IsNullOrWhiteSpace(requestDto.Status))
            {
                var nextStatus = requestDto.Status.Trim();
                product.Status = nextStatus;
            }

            product.UpdatedAt = GenerateExtentions.Now;
            product.UpdatedBy = UserId.ToString();
            product.PublishedAt = string.Equals(product.Status, ProductConstants.Status.Published, StringComparison.OrdinalIgnoreCase)
                ? product.PublishedAt ?? GenerateExtentions.Now
                : null;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.IsDeleted || product.SellerId != UserId)
                throw new BusinessException(ErrorCode.ProductNotFound, "Sản phẩm không tồn tại.");

            product.IsDeleted = true;
            product.IsActive = false;
            product.Status = ProductConstants.Status.Archived;
            product.PublishedAt = null;
            product.UpdatedAt = GenerateExtentions.Now;
            product.UpdatedBy = UserId.ToString();

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteProductByItemIdAsync(Guid itemId)
        {
            return DeleteProductAsync(itemId);
        }

        public async Task<CategoryTreeResDto> GetCategoryTreeAsync(CategoryTreeReqDto requestDto)
        {
            var categories = await _productRepository.GetCategoryTreeAsync(requestDto.IncludeDisabled ?? false);

            var nodes = categories.ToDictionary(
                x => x.Id,
                x => new CategoryNodeDto
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Slug = x.Slug,
                    Level = x.Level,
                    ParentId = x.ParentId?.ToString(),
                    Children = new List<CategoryNodeDto>()
                });

            foreach (var node in nodes.Values)
            {
                if (node.ParentId != null && nodes.TryGetValue(Guid.Parse(node.ParentId), out var parent))
                {
                    parent.Children!.Add(node);
                }
            }

            var result = nodes.Values
                .Where(x => x.ParentId == null || !nodes.ContainsKey(Guid.Parse(x.ParentId)))
                .ToList();

            return new CategoryTreeResDto
            {
                Categories = result
            };
        }
    }
}
