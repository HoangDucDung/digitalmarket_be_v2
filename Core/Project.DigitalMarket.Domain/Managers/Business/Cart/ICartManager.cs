using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Managers.Business.Cart
{
    /// <summary>
    /// Manager xử lý các nghiệp vụ liên quan đến Giỏ hàng (Domain layer)
    /// </summary>
    public interface ICartManager
    {
        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng của người dùng
        /// </summary>
        Task AddToCartAsync(Guid userId, Guid productId, int quantity);

        /// <summary>
        /// Cập nhật số lượng của một mục trong giỏ hàng
        /// </summary>
        Task UpdateQuantityAsync(Guid userId, Guid cartItemId, int quantity);

        /// <summary>
        /// Xóa một mục khỏi giỏ hàng
        /// </summary>
        Task RemoveFromCartAsync(Guid userId, Guid cartItemId);

        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm trong giỏ hàng của người dùng
        /// </summary>
        Task<List<CartItemEntity>> GetUserCartAsync(Guid userId);

        /// <summary>
        /// Xóa sạch tất cả các mục trong giỏ hàng của người dùng
        /// </summary>
        Task ClearCartAsync(Guid userId);
    }
}
