using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Entities.Base;
using System;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    /// <summary>
    /// Thực thể bình luận/đánh giá sản phẩm.
    /// </summary>
    public class CommentEntity : BaseEntity
    {
        /// <summary>
        /// ID của sản phẩm được bình luận.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// ID của người dùng để lại bình luận.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Nội dung bình luận.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Số sao đánh giá (1-5).
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Danh sách URL hình ảnh đi kèm (dạng JSON string).
        /// </summary>
        public string? ImageUrls { get; set; }

        /// <summary>
        /// Navigation property tới sản phẩm.
        /// </summary>
        public virtual ProductEntity? Product { get; set; }

        /// <summary>
        /// Navigation property tới người dùng.
        /// </summary>
        public virtual UserEntity? User { get; set; }
    }
}
