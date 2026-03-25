using Project.DigitalMarket.Application.Contract.DTOs.Business.Order;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Order
{
    /// <summary>
    /// Service quản lý luồng đặt hàng và thanh toán.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Thanh toán toàn bộ giỏ hàng cho người dùng hiện tại.
        /// </summary>
        /// <param name="req">Thông tin thanh toán và ghi chú</param>
        /// <returns>Bản ghi đơn hàng đã tạo</returns>
        Task<OrderResultDto> CheckoutCartAsync(CheckoutCartReqDto req);

        /// <summary>
        /// Mua sản phẩm trực tiếp từ trang chi tiết (Buy Now).
        /// </summary>
        /// <param name="req">Thông tin sản phẩm, số lượng và thanh toán</param>
        /// <returns>Bản ghi đơn hàng đã tạo</returns>
        Task<OrderResultDto> DirectPurchaseAsync(DirectPurchaseReqDto req);

        /// <summary>
        /// Lấy danh sách lịch sử đơn hàng của người dùng hiện tại.
        /// </summary>
        /// <returns>Danh sách đơn hàng tối giản</returns>
        Task<List<OrderResultDto>> GetMyOrdersAsync();

        /// <summary>
        /// Lấy chi tiết một đơn hàng kèm danh sách sản phẩm.
        /// </summary>
        /// <param name="orderId">ID đơn hàng cần xem</param>
        /// <returns>Thông tin chi tiết đơn hàng</returns>
        Task<OrderResultDto> GetOrderDetailAsync(Guid orderId);

        /// <summary>
        /// Hủy đơn hàng đang ở trạng thái chờ (Pending).
        /// </summary>
        /// <param name="orderId">ID đơn hàng cần hủy</param>
        Task CancelOrderAsync(Guid orderId);
    }
}
