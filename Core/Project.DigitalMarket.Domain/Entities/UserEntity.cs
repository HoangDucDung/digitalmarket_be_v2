using Microsoft.AspNetCore.Identity;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// User entity kế thừa IdentityUser, bổ sung thông tin tùy chỉnh
    /// </summary>
    public class UserEntity : IdentityUser<Guid>
    {
        /// <summary>
        /// Họ và tên đầy đủ
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Ngày tạo tài khoản
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thông tin vai trò của người dùng (ví dụ: "Admin", "User", ...)
        /// </summary>
        public string Role { get; set; } = "User";
    }
}
