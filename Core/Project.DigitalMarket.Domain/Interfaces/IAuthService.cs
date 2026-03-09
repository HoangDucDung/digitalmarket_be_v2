using Project.DigitalMarket.Domain.DTOs;

namespace Project.DigitalMarket.Domain.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ xác thực
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="registerDto">Thông tin đăng ký</param>
        /// <returns>Kết quả đăng ký</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginDto">Thông tin đăng nhập</param>
        /// <returns>JWT token và thông tin user</returns>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
