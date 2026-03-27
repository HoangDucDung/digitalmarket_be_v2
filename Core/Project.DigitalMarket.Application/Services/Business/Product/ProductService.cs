using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Domain.Managers.Business.Product;
using Project.DigitalMarket.Domain.Models.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace Project.DigitalMarket.Application.Services.Business.Product
{
    public class ProductService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IProductService
    {
        private IProductManager _productManager => _lazyloadProvider.LazyGetRequiredService<IProductManager>();

        public async Task<DiscoveryResDto> GetDailyDiscoverAsync(DiscoveryReqDto discoveryRequestDto)
        {
            var result = await _productManager.GetDailyDiscoverAsync(new ProductDiscoveryReq
            {
                Limit = discoveryRequestDto.Limit.Value,
                Offset = discoveryRequestDto.Offset.Value
            });

            var feeds = result.Items.Select(p => new FeedItemDto
            {
                CentralisedItemCard = new CentralisedItemCardDto
                {
                    ItemData = new
                    {
                        Itemid = p.ProductId,
                        Shopid = p.SellerId,
                        Price = p.Price,
                        OriginalPrice = p.OriginalPrice,
                        Discount = p.DiscountPercent
                    },
                    ItemCardDisplayedAsset = new
                    {
                        Name = p.Name,
                        ShopName = p.ShopName,
                        SoldCountText = p.SoldCount,
                        Rating = p.RatingCount,
                        Image = $"https://localhost:7097/api/files/{p.ThumbnailFileId}"
                    }
                }
            }).ToList();

            return new DiscoveryResDto
            {
                Feeds = feeds,
                FeedTotal = result.Total,
                ReqId = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<ProductDetailResDto?> GetProductDetailAsync(ProductDetailReqDto requestDto)
        {
            var result = await _productManager.GetProductDetailAsync(new ProductDetailReq
            {
                ItemId = requestDto.ItemId,
                ShopId = requestDto.SellerId
            });
            if (result is null) return null;

            //return new ProductDetailResDto
            //{
            //    ProductId = result.ProductId,
            //    Name = result.Name,
            //    Slug = result.Slug,
            //    Description = result.Description,
            //    Material = result.Material,
            //    Currency = result.Currency,
            //    Status = result.Status,
            //    CategoryName = result.CategoryName,
            //    BrandName = result.BrandName,
            //    EnableVariation = result.EnableVariation,
            //    MinPrice = result.MinPrice,
            //    MaxPrice = result.MaxPrice,
            //    SoldCount = result.SoldCount,
            //    AvgRating = result.AvgRating,
            //    RatingCount = result.RatingCount,
            //    SellerId = result.SellerId,
            //    ShopName = result.ShopName,
            //    Images = result.Images.Select(i => new ProductDetailImageDto
            //    {
            //        FileId = i.FileId,
            //        SortOrder = i.SortOrder,
            //        IsPrimary = i.IsPrimary
            //    }).ToList(),
            //    Variants = result.Variants.Select(v => new ProductDetailVariantDto
            //    {
            //        VariantId = v.VariantId,
            //        VariantName = v.VariantName,
            //        Sku = v.Sku,
            //        Price = v.Price,
            //        OriginalPrice = v.OriginalPrice,
            //        StockQuantity = v.StockQuantity,
            //        Attributes = v.Attributes.Select(a => new ProductDetailVariantAttributeDto
            //        {
            //            AttributeName = a.AttributeName,
            //            AttributeValue = a.AttributeValue,
            //            AttributeOrder = a.AttributeOrder
            //        }).ToList()
            //    }).ToList(),
            //    ReqId = Guid.NewGuid().ToString("N")
            //};

            return new ProductDetailResDto
            {
                Item = new ProductItemDetailDto
                {
                    ItemId = result.ProductId,
                    ShopId = result.SellerId,
                    Title = result.Name,
                    Image = $"https://localhost:7097/api/files/{result.Images[0].FileId}",
                },
                ItemVariants = result.Variants.Select(v => new ProductDetailVariantDto
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
                ProductReview = new ProductReviewDetailDto
                {
                    RatingStar = result.RatingCount,
                    TotalRatingCount = 0,
                    CmtCount = 0,
                    HistoricalSold = result.SoldCount
                },
                ShopDetailed = new ProductShopDetailDto
                {
                    ShopId = result.ProductId,
                    Name = result.ShopName,
                    //ShopLocation = result.ShopLocation,
                    //RatingStar = result.RatingStar
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

            //var detail = await _productManager.GetProductDetailAsync(new ProductDetailReq { ProductId = productId });
            return new ProductCreateResDto
            {
                ProductId = productId,
                //Slug = detail?.Slug ?? string.Empty,
                //Status = detail?.Status ?? string.Empty,
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

        public async Task<CategoryTreeResDto> GetCategoryTreeAsync(CategoryTreeReqDto requestDto)
        {
            var categories = await _productManager.GetCategoryTreeAsync(new CategoryTreeReq
            {
                IncludeDisabled = requestDto.IncludeDisabled ?? false
            });

            return new CategoryTreeResDto
            {
                Categories = categories.Select(MapCategoryNode).ToList()
            };
        }

        private static CategoryNodeDto MapCategoryNode(CategoryNodeResult node)
        {
            return new CategoryNodeDto
            {
                Id = node.Id.ToString(),
                Name = node.Name,
                Slug = node.Slug,
                Level = node.Level,
                ParentId = node.ParentId?.ToString(),
                IsLeaf = node.IsLeaf,
                SortOrder = node.SortOrder,
                Children = node.Children?.Select(MapCategoryNode).ToList()
            };
        }
    }
}
