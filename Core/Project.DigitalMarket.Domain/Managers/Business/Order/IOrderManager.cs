using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Managers.Business.Order
{
    /// <summary>
    /// Manager quản lý các nghiệp vụ liên quan đến Đơn hàng (Domain layer)
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Mua các sản phẩm đã chọn trong giỏ hàng
        /// </summary>
        Task<OrderEntity> CheckoutCartAsync(Guid userId, string paymentMethod, string? note);

        /// <summary>
        /// Mua trực tiếp 1 sản phẩm không qua giỏ hàng
        /// </summary>
        Task<OrderEntity> DirectPurchaseAsync(Guid userId, Guid productId, int quantity, string paymentMethod, string? note);

        /// <summary>
        /// Lấy toàn bộ lịch sử đơn hàng của người dùng
        /// </summary>
        Task<List<OrderEntity>> GetUserOrdersAsync(Guid userId);

        /// <summary>
        /// Lấy thông tin chi tiết của một đơn hàng
        /// </summary>
        Task<OrderEntity> GetOrderDetailAsync(Guid userId, Guid orderId);

        /// <summary>
        /// Hủy đơn hàng ở trạng thái chờ
        /// </summary>
        Task CancelOrderAsync(Guid userId, Guid orderId);
    }
}
