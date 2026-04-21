using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Cart
{
    public interface ICartRepository : IRepositoryBase<CartItemEntity>
    {
        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm trong giỏ hàng của người dùng kèm thông tin Product và Images
        /// </summary>
        Task<List<CartItemEntity>> GetCartByUserIdAsync(Guid userId);

        /// <summary>
        /// Lấy một mục cụ thể trong giỏ hàng của người dùng
        /// </summary>
        Task<CartItemEntity?> GetCartItemAsync(Guid userId, Guid cartItemId);

        /// <summary>
        /// Lấy mục giỏ hàng theo Sản phẩm của một người dùng
        /// </summary>
        Task<CartItemEntity?> GetCartItemByProductAsync(Guid userId, Guid productId);

        /// <summary>
        /// Xóa sạch tất cả các mục trong giỏ hàng của người dùng
        /// </summary>
        Task ClearCartAsync(Guid userId);
    }
}
