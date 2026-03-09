using System.ComponentModel.DataAnnotations;

namespace Project.DigitalMarket.Domain.DTOs
{
    /// <summary>
    /// DTO cho request đăng nhập
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = string.Empty;
    }
}
