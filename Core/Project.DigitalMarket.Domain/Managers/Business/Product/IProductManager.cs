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
        /// Thêm sản phẩm mới (Nghiệp vụ đặc thù: sinh slug, xử lý variant, validation phức tạp)
        /// </summary>
        Task<Guid> AddProductAsync(ProductCreateReq request);
    }
}
