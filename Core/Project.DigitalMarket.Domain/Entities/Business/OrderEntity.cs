using Project.DigitalMarket.Domain.Entities.Base;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Share.Constants.Business;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    /// <summary>
    /// Bản ghi thông tin đơn hàng của khách hàng.
    /// </summary>
    public class OrderEntity : BaseEntity
    {
        /// <summary>
        /// ID của người mua (User)
        /// </summary>
        public Guid BuyerId { get; set; }

        /// <summary>
        /// Mã đơn hàng dễ nhận diện (VD: ORD12345678)
        /// </summary>
        public string OrderCode { get; set; } = string.Empty;

        /// <summary>
        /// Tổng cộng tất cả các Item
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Tổng giảm giá
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// Giá trị cuối cùng cần thanh toán
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Phí thanh toán (nếu có)
        /// </summary>
        public decimal ProcessingFee { get; set; }

        /// <summary>
        /// Trạng thái của đơn hàng (Pending, Processing, Completed, Cancelled...)
        /// </summary>
        public string Status { get; set; } = OrderConstants.Status.Pending;

        /// <summary>
        /// Hình thức thanh toán (InternalBalance, BankTransfer...)
        /// </summary>
        public string PaymentMethod { get; set; } = OrderConstants.PaymentMethod.InternalBalance;

        /// <summary>
        /// Ngày thanh toán (nếu có)
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Ghi chú người mua
        /// </summary>
        public string? BuyerNote { get; set; }

        public virtual ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }
}
