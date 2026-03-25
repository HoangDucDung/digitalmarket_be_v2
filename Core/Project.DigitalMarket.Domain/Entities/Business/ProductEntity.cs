using Project.DigitalMarket.Domain.Entities.Base;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using System.ComponentModel.DataAnnotations;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    /// <summary>
    /// Sản phẩm được hiển thị trên luồng khám phá.
    /// </summary>
    public class ProductEntity : BaseEntity
    {
        /// <summary>
        /// ID của người bán (Seller)
        /// </summary>
        public Guid SellerId { get; set; }

        /// <summary>
        /// ID của danh mục sản phẩm (Category)
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// ID của thương hiệu sản phẩm (Brand)
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Slug phục vụ cho việc tạo URL SEO
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// URL hình ảnh sản phẩm
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Tên cửa hàng bán sản phẩm này
        /// </summary>
        public string ShopName { get; set; } = string.Empty;

        /// <summary>
        /// Địa điểm cửa hàng (mặc định: Ho Chi Minh)
        /// </summary>
        public string ShopLocation { get; set; } = "Ho Chi Minh";

        /// <summary>
        /// Đơn vị tiền tệ (mặc định: VND)
        /// </summary>
        public string Currency { get; set; } = "VND";

        /// <summary>
        /// Giá gốc của sản phẩm
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Giá bán khuyến mãi (nếu có)
        /// </summary>
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// Phần trăm giảm giá
        /// </summary>
        public int DiscountPercent { get; set; }

        /// <summary>
        /// Số lượng sản phẩm đã bán
        /// </summary>
        public int SoldCount { get; set; }

        /// <summary>
        /// Điểm đánh giá trung bình (1-5 sao)
        /// </summary>
        public decimal RatingAverage { get; set; }

        /// <summary>
        /// Định danh gói danh mục cho feed khám phá
        /// </summary>
        public string CategoryBundle { get; set; } = "daily_discover_main";

        /// <summary>
        /// Trạng thái của sản phẩm (Draft, Active, Hidden, Banned)
        /// </summary>
        public string Status { get; set; } = ProductConstants.Status.Active;

        /// <summary>
        /// Thời gian sản phẩm được công khai
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Sản phẩm có đang hoạt động hay không
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Sản phẩm có được làm nổi bật (Featured) hay không
        /// </summary>
        public bool IsFeatured { get; set; } = false;
    }
}
