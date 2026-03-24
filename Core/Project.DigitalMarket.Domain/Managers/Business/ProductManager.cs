using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Models.Business;
using Project.DigitalMarket.Domain.Repositories.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Domain.Managers.Business
{
    public class ProductManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IProductManager
    {
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        public async Task<ProductDiscoveryResult> GetDailyDiscoverAsync(ProductDiscoveryReq request)
        {
            var limit = Math.Clamp(request.Limit, 1, 60);
            var offset = Math.Max(0, request.Offset);
            var bundle = string.IsNullOrWhiteSpace(request.Bundle) ? "daily_discover_main" : request.Bundle.Trim();

            var query = _productRepository.GetDiscoverQuery();
            if (!string.Equals(bundle, "daily_discover_main", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.CategoryBundle == bundle);
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.SoldCount)
                .ThenByDescending(x => x.DiscountPercent)
                .ThenByDescending(x => x.RatingAverage)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .Select(x => new ProductDiscoveryItem
                {
                    ItemId = x.Id,
                    ShopId = x.SellerId,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    ShopName = x.ShopName,
                    ShopLocation = x.ShopLocation,
                    OriginalPrice = x.OriginalPrice,
                    FinalPrice = x.SalePrice ?? (x.OriginalPrice * (100 - x.DiscountPercent) / 100),
                    DiscountPercent = x.DiscountPercent,
                    SoldCount = x.SoldCount,
                    RatingAverage = x.RatingAverage,
                    IsFeatured = x.IsFeatured
                })
                .ToListAsync();

            return new ProductDiscoveryResult
            {
                Items = items,
                Total = total
            };
        }

        public async Task<ProductDetailResult?> GetProductDetailAsync(ProductDetailReq request)
        {
            return await _productRepository.GetDiscoverQuery()
                .Where(x => x.Id == request.ItemId && x.SellerId == request.ShopId)
                .Select(x => new ProductDetailResult
                {
                    ItemId = x.Id,
                    ShopId = x.SellerId,
                    Title = x.Name,
                    Image = x.ImageUrl,
                    Currency = x.Currency,
                    DiscountPercent = x.DiscountPercent,
                    Price = x.SalePrice ?? (x.OriginalPrice * (100 - x.DiscountPercent) / 100),
                    PriceBeforeDiscount = x.OriginalPrice,
                    RatingStar = x.RatingAverage,
                    SoldCount = x.SoldCount,
                    ShopName = x.ShopName,
                    ShopLocation = x.ShopLocation,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}
