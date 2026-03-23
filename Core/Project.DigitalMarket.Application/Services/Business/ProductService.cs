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
        public Task<DiscoveryResponseDto> GetDailyDiscoverAsync(DiscoveryRequestDto discoveryRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
