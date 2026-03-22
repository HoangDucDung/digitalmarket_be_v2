using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// Log các hoạt động nhạy cảm giúp bảo mật và giải quyết tranh chấp
    /// </summary>
    public class UserAuditLogEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID người dùng thực hiện hành động
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Hành động thực hiện (LOGIN_SUCCESS, CHANGE_PASSWORD...)
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ IP (Hỗ trợ cả IPv4 và IPv6)
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// Thông tin trình duyệt/App
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Thời gian xảy ra hành động
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        /// <summary>
        /// Dữ liệu bổ sung dạng JSON nếu cần lưu thêm thông tin chi tiết
        /// </summary>
        public string? Metadata { get; set; }

        /// <summary>
        /// Navigation property tới UserEntity
        /// </summary>
        public virtual UserEntity User { get; set; } = null!;
    }
}
