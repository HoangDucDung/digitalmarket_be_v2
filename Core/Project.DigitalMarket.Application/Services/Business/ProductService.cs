using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Domain.Managers.Business;
using Project.DigitalMarket.Domain.Models.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business
{
    /// <summary>
    /// Service xử lý các nghiệp vụ liên quan đến Product
    /// </summary>
    public class ProductService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IProductService
    {
        private IProductManager _productManager => _lazyloadProvider.LazyGetRequiredService<IProductManager>();

        public async Task<DiscoveryResponseDto> GetDailyDiscoverAsync(DiscoveryRequestDto discoveryRequestDto)
        {
            var req = _mapper.Map<ProductDiscoveryReq>(discoveryRequestDto);
            var result = await _productManager.GetDailyDiscoverAsync(req);

            var feeds = result.Items.Select(p => new FeedItemDto
            {
                CentralisedItemCard = new CentralisedItemCardDto
                {
                    ItemData = new
                    {
                        Itemid = p.ItemId,
                        Shopid = p.ShopId,
                        Price = p.FinalPrice,
                        OriginalPrice = p.OriginalPrice,
                        Discount = p.DiscountPercent
                    },
                    ItemCardDisplayedAsset = new
                    {
                        Name = p.Name,
                        Image = p.ImageUrl,
                        ShopName = p.ShopName,
                        ShopLocation = p.ShopLocation,
                        SoldCountText = FormatSoldCount(p.SoldCount)
                    }
                }
            }).ToList();

            return new DiscoveryResponseDto
            {
                Feeds = feeds,
                FeedTotal = result.Total,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        private static string FormatSoldCount(int soldCount)
        {
            if (soldCount >= 1000)
            {
                return $"{soldCount / 1000.0:0.#}k da ban";
            }

            return $"{soldCount} da ban";
        }

        public async Task<ProductDetailResponseDto?> GetProductDetailAsync(ProductDetailRequestDto requestDto)
        {
            var result = await _productManager.GetProductDetailAsync(new ProductDetailReq
            {
                ItemId = requestDto.ItemId,
                ShopId = requestDto.ShopId
            });

            if (result is null)
            {
                return null;
            }

            return new ProductDetailResponseDto
            {
                Item = new ProductItemDetailDto
                {
                    ItemId = result.ItemId,
                    ShopId = result.ShopId,
                    Title = result.Title,
                    Image = result.Image,
                    Currency = result.Currency,
                    ShowDiscount = result.DiscountPercent,
                    Price = result.Price,
                    PriceBeforeDiscount = result.PriceBeforeDiscount,
                    RatingStar = result.RatingStar,
                    ShopLocation = result.ShopLocation,
                    HistoricalSold = result.SoldCount,
                    CTime = result.CreatedAt,
                    IsFreeShipping = true
                },
                ProductPrice = new ProductPriceDetailDto
                {
                    Discount = result.DiscountPercent,
                    Price = result.Price,
                    PriceBeforeDiscount = result.PriceBeforeDiscount,
                    HidePrice = false
                },
                ProductReview = new ProductReviewDetailDto
                {
                    RatingStar = result.RatingStar,
                    TotalRatingCount = 0,
                    CmtCount = 0,
                    HistoricalSold = result.SoldCount
                },
                ShopDetailed = new ProductShopDetailDto
                {
                    ShopId = result.ShopId,
                    Name = result.ShopName,
                    ShopLocation = result.ShopLocation,
                    RatingStar = result.RatingStar
                },
                ReqId = Guid.NewGuid().ToString("N")
            };
        }
    }
}
