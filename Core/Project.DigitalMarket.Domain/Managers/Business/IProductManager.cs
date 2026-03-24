using Project.DigitalMarket.Domain.Models.Business;

namespace Project.DigitalMarket.Domain.Managers.Business
{
    public interface IProductManager
    {
        Task<ProductDiscoveryResult> GetDailyDiscoverAsync(ProductDiscoveryReq request);
        Task<ProductDetailResult?> GetProductDetailAsync(ProductDetailReq request);
    }
}
