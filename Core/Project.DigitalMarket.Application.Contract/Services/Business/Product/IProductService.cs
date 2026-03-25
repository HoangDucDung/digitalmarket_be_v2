using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Product
{
    public interface IProductService
    {
        Task<DiscoveryResDto> GetDailyDiscoverAsync(DiscoveryReqDto discoveryRequestDto);
        Task<ProductDetailResDto?> GetProductDetailAsync(ProductDetailReqDto requestDto);
    }
}
