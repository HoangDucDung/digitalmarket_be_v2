using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Managers.Business.Order
{
    /// <summary>
    /// Manager quản lý các nghiệp vụ liên quan đến Đơn hàng (Domain layer)
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Mua các sản phẩm đã chọn trong giỏ hàng (Nghiệp vụ đặc thù: liên kết Cart, Product, Wallet)
        /// </summary>
        Task<OrderEntity> CheckoutCartAsync(Guid userId, string paymentMethod, string? note);

        /// <summary>
        /// Mua trực tiếp 1 sản phẩm (Nghiệp vụ đặc thù: liên kết Product, Wallet)
        /// </summary>
        Task<OrderEntity> DirectPurchaseAsync(Guid userId, Guid productId, int quantity, string paymentMethod, string? note);
    }
}
