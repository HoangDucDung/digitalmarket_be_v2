using Project.DigitalMarket.Domain.Entities.Base;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    /// <summary>
    /// Các mục trong bản ghi đơn hàng cố định dữ liệu tại thời điểm mua.
    /// </summary>
    public class OrderItemEntity : BaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public virtual ProductEntity Product { get; set; } = null!;

        /// <summary>
        /// Tên sản phẩm lưu lại đề phòng bị thay đổi/xóa sau này
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng thực tế đã mua
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Đơn giá lúc mua (Sau khi đã áp khuyến mãi)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Giá gốc tham chiếu
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Thành tiền (Quantity * UnitPrice)
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Các thông tin đi kèm từ sản phẩm số (VD: Serial key, link...)
        /// </summary>
        public string? DeliveryInfo { get; set; }
    }
}
