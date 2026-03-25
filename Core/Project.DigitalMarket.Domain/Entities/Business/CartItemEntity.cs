using Project.DigitalMarket.Domain.Entities.Base;
using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    /// <summary>
    /// Các mục trong giỏ hàng lưu trữ tạm cho tới khi mua.
    /// </summary>
    public class CartItemEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public virtual ProductEntity Product { get; set; } = null!;

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Giá tại thời điểm thêm vào giỏ hàng (giá niêm yết/khuyến mại)
        /// </summary>
        public decimal ReferencePrice { get; set; }

        /// <summary>
        /// Sản phẩm đã chọn mua (Ready to bill)
        /// </summary>
        public bool IsSelected { get; set; } = true;
    }
}
