using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Managers.Business.Cart
{
    /// <summary>
    /// Manager xử lý các nghiệp vụ liên quan đến Giỏ hàng (Domain layer)
    /// </summary>
    public interface ICartManager
    {
        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng (Nghiệp vụ đặc thù: check chính chủ, lấy giá tham chiếu)
        /// </summary>
        Task AddToCartAsync(Guid userId, Guid productId, int quantity);
    }
}
