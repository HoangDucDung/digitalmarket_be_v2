using Project.DigitalMarket.Application.Contract.DTOs.Business.Cart;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Cart
{
    /// <summary>
    /// Service quản lý giỏ hàng tạm thời của người dùng.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng.
        /// </summary>
        Task AddToCartAsync(Guid productId, int quantity);

        /// <summary>
        /// Cập nhật số lượng của một mục trong giỏ hàng.
        /// </summary>
        Task UpdateQuantityAsync(Guid cartItemId, int quantity);

        /// <summary>
        /// Xóa một mục cụ thể khỏi giỏ hàng.
        /// </summary>
        Task RemoveFromCartAsync(Guid cartItemId);

        /// <summary>
        /// Lấy toàn bộ danh sách giỏ hàng của người dùng hiện tại.
        /// </summary>
        Task<List<CartItemResultDto>> GetMyCartAsync();

        /// <summary>
        /// Xóa sạch giỏ hàng.
        /// </summary>
        Task ClearCartAsync();
    }
}
