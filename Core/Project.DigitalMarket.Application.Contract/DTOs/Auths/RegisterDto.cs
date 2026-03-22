using System.ComponentModel.DataAnnotations;

namespace Project.DigitalMarket.Application.Contract.DTOs.Auths
{
    /// <summary>
    /// DTO cho request đăng ký tài khoản
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Email đăng ký
        /// </summary>
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu (tối thiểu 6 ký tự, có chữ hoa và số)
        /// </summary>
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Họ và tên đầy đủ
        /// </summary>
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string FullName { get; set; } = string.Empty;
    }
}
