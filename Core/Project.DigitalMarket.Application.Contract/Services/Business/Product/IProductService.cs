using System;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Product
{
    public interface IProductService
    {
        Task<DiscoveryResDto> GetDailyDiscoverAsync(DiscoveryReqDto discoveryRequestDto);
        Task<ProductDetailResDto?> GetProductDetailAsync(ProductDetailReqDto requestDto);

        /// <summary>
        /// Thêm sản phẩm mới (tạo/cập nhật theo kiểu admin).
        /// </summary>
        Task<ProductCreateResDto> AddProductAsync(ProductCreateReqDto requestDto);

        /// <summary>
        /// Cập nhật sản phẩm (patch) của seller hiện tại.
        /// </summary>
        Task<bool> UpdateProductAsync(ProductUpdateReqDto requestDto);

        /// <summary>
        /// Xóa (soft-delete) sản phẩm của seller hiện tại.
        /// </summary>
        Task<bool> DeleteProductAsync(Guid productId);

        Task<CategoryTreeResDto> GetCategoryTreeAsync(CategoryTreeReqDto requestDto);
    }
}
