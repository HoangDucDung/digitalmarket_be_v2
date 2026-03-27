using System;
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

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        Task<Guid> AddProductAsync(ProductCreateReq request);

        /// <summary>
        /// Cập nhật sản phẩm (patch) của seller
        /// </summary>
        Task<bool> UpdateProductAsync(ProductUpdateReq request);

        /// <summary>
        /// Xóa (soft-delete) sản phẩm của seller
        /// </summary>
        Task<bool> DeleteProductAsync(Guid sellerId, Guid productId);
    }
}
