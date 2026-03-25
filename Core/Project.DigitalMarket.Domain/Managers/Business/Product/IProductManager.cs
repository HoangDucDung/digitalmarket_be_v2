using Project.DigitalMarket.Domain.Models.Business.Product;

namespace Project.DigitalMarket.Domain.Managers.Business.Product
{
    /// <summary>
    /// Manager quản lý các nghiệp vụ liên quan đến Sản phẩm (Domain layer)
    /// </summary>
    public interface IProductManager
    {
        /// <summary>
        /// Lấy danh sách sản phẩm cho feed khám phá hàng ngày
        /// </summary>
        Task<ProductDiscoveryResult> GetDailyDiscoverAsync(ProductDiscoveryReq request);

        /// <summary>
        /// Lấy thông tin chi tiết của một sản phẩm bất kỳ
        /// </summary>
        Task<ProductDetailResult?> GetProductDetailAsync(ProductDetailReq request);
    }
}
