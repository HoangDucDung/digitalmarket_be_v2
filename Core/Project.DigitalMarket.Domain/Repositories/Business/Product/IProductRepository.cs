using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Product
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        /// <summary>
        /// Lấy danh sách sản phẩm khám phá có phân trang và tìm kiếm
        /// </summary>
        /// <param name="limit">Số lượng bản ghi tối đa</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <param name="keyword">Từ khóa tìm kiếm (theo tên)</param>
        /// <returns>Danh sách sản phẩm và tổng số lượng bản ghi</returns>
        Task<(List<ProductEntity> Items, int Total)> GetPagedDiscoveryAsync(int limit, int offset, string? keyword);

        /// <summary>
        /// Lấy thông tin chi tiết sản phẩm kèm các bảng liên quan (Images, Variants, Rating...)
        /// </summary>
        Task<ProductEntity?> GetProductDetailByIdAsync(Guid productId);

        /// <summary>
        /// Lấy cây danh mục sản phẩm
        /// </summary>
        /// <param name="includeDisabled">Có bao gồm cả các danh mục bị ẩn không</param>
        Task<List<CategoryEntity>> GetCategoryTreeAsync(bool includeDisabled);

        /// <summary>
        /// Kiểm tra Slug sản phẩm đã tồn tại hay chưa
        /// </summary>
        Task<bool> IsSlugExistsAsync(string slug);
        
        Task<CategoryEntity?> ResolveCategoryAsync(string categoryNameOrSlug);
        Task<BrandEntity?> ResolveBrandAsync(string? brandNameOrSlug);

        /// <summary>
        /// Lấy thông tin sản phẩm đang hoạt động kèm thông tin biến thể
        /// </summary>
        Task<ProductEntity?> GetActiveWithVariantsByIdAsync(Guid productId);
    }
}
