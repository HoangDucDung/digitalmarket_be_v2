using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Ngày tạo bản ghi
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        /// <summary>
        /// Người tạo (Id hoặc Tên)
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật bản ghi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Người cập nhật (Id hoặc Tên)
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Đánh dấu là đã xóa (Soft Delete)
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
