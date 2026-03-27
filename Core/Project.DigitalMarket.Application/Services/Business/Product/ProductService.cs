using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Domain.Managers.Business.Product;
using Project.DigitalMarket.Domain.Models.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business.Product
{
    public class ProductService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IProductService
    {
        private IProductManager _productManager => _lazyloadProvider.LazyGetRequiredService<IProductManager>();

        public async Task<DiscoveryResDto> GetDailyDiscoverAsync(DiscoveryReqDto discoveryRequestDto)
        {
            var result = await _productManager.GetDailyDiscoverAsync(new ProductDiscoveryReq
            {
                CentralisedItemCard = new CentralisedItemCardDto
                {
                    ItemData = new
                    {
                        Itemid = p.ItemId,
                        Shopid = p.ShopId,
                        Price = p.FinalPrice,
                        p.OriginalPrice,
                        Discount = p.DiscountPercent
                    },
                    ItemCardDisplayedAsset = new
                    {
                        p.Name,
                        Image = p.ImageUrl,
                        p.ShopName,
                        p.ShopLocation,
                        SoldCountText = FormatSoldCount(p.SoldCount),
                        Rating = p.RatingAverage
                    }
                }
            }).ToList();

            return new DiscoveryResDto
            {
                Items = result.Items.Select(x => new DailyDiscoverItemDto
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Slug = x.Slug,
                    ThumbnailFileId = x.ThumbnailFileId,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                    DiscountPercent = x.DiscountPercent,
                    SoldCount = x.SoldCount,
                    AvgRating = x.AvgRating,
                    RatingCount = x.RatingCount,
                    SellerId = x.SellerId,
                    ShopName = x.ShopName
                }).ToList(),
                Total = result.Total,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<ProductDetailResDto?> GetProductDetailAsync(ProductDetailReqDto requestDto)
        {
            var result = await _productManager.GetProductDetailAsync(new ProductDetailReq
            {
                ProductId = requestDto.ProductId
            });
            if (result is null) return null;

            return new ProductDetailResDto
            {
                ProductId = result.ProductId,
                Name = result.Name,
                Slug = result.Slug,
                Description = result.Description,
                Material = result.Material,
                Currency = result.Currency,
                Status = result.Status,
                CategoryName = result.CategoryName,
                BrandName = result.BrandName,
                EnableVariation = result.EnableVariation,
                MinPrice = result.MinPrice,
                MaxPrice = result.MaxPrice,
                SoldCount = result.SoldCount,
                AvgRating = result.AvgRating,
                RatingCount = result.RatingCount,
                SellerId = result.SellerId,
                ShopName = result.ShopName,
                Images = result.Images.Select(i => new ProductDetailImageDto
                {
                    FileId = i.FileId,
                    SortOrder = i.SortOrder,
                    IsPrimary = i.IsPrimary
                }).ToList(),
                Variants = result.Variants.Select(v => new ProductDetailVariantDto
                {
                    VariantId = v.VariantId,
                    VariantName = v.VariantName,
                    Sku = v.Sku,
                    Price = v.Price,
                    OriginalPrice = v.OriginalPrice,
                    StockQuantity = v.StockQuantity,
                    Attributes = v.Attributes.Select(a => new ProductDetailVariantAttributeDto
                    {
                        AttributeName = a.AttributeName,
                        AttributeValue = a.AttributeValue,
                        AttributeOrder = a.AttributeOrder
                    }).ToList()
                }).ToList(),
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<ProductCreateResDto> AddProductAsync(ProductCreateReqDto requestDto)
        {
            var productId = await _productManager.AddProductAsync(new ProductCreateReq
            {
                SellerId = UserId,
                Name = requestDto.Name,
                Category = requestDto.Category,
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

            var detail = await _productManager.GetProductDetailAsync(new ProductDetailReq { ProductId = productId });
            return new ProductCreateResDto
            {
                ProductId = productId,
                Slug = detail?.Slug ?? string.Empty,
                Status = detail?.Status ?? string.Empty,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public Task<bool> UpdateProductAsync(ProductUpdateReqDto requestDto)
        {
            return _productManager.UpdateProductAsync(new ProductUpdateReq
            {
                SellerId = UserId,
                ProductId = requestDto.ProductId,
                Name = requestDto.Name,
                Status = requestDto.Status
            });
        }

        public Task<bool> DeleteProductAsync(Guid productId)
        {
            return _productManager.DeleteProductAsync(UserId, productId);
        }
    }
}
