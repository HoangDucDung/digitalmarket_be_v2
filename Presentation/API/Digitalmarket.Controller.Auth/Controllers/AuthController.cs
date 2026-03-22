using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Auths;
using Project.DigitalMarket.Application.Contract.Services.Auths;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Auth.Controllers
{
    /// <summary>
    /// Controller xử lý xác thực: đăng ký và đăng nhập
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IAuthService _authService => _lazyloadProvider.LazyGetRequiredService<IAuthService>();

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="registerDto">Thông tin đăng ký (Email, Password, FullName)</param>
        /// <returns>JWT token và thông tin user</returns>
        /// <response code="200">Đăng ký thành công, trả về token</response>
        /// <response code="422">Dữ liệu không hợp lệ hoặc email đã tồn tại</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginDto">Thông tin đăng nhập (Email, Password)</param>
        /// <returns>JWT token và thông tin user</returns>
        /// <response code="200">Đăng nhập thành công, trả về token</response>
        /// <response code="401">Email hoặc mật khẩu không đúng</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        /// <summary>
        /// Xác thực email sau khi đăng ký
        /// </summary>
        [HttpPost("verify-email")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            await _authService.VerifyEmailAsync(dto);
            return Ok(new { Message = "Xác thực email thành công." });
        }

        /// <summary>
        /// Xác thực đăng nhập 2FA
        /// </summary>
        [HttpPost("verify-2fa-login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Verify2FALogin([FromBody] Verify2FALoginDto dto)
        {
            var result = await _authService.Verify2FALoginAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Gửi mã kích hoạt 2FA (yêu cầu đăng nhập)
        /// </summary>
        [HttpPost("enable-2fa")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Enable2FA()
        {
            await _authService.Enable2FAAsync(UserContext.UserId);
            return Ok(new { Message = "Mã xác nhận đã được gửi đến email của bạn." });
        }

        /// <summary>
        /// Xác nhận mã và bật 2FA (yêu cầu đăng nhập)
        /// </summary>
        [HttpPost("confirm-enable-2fa")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEnable2FA([FromBody] ConfirmEnable2FADto dto)
        {
            await _authService.ConfirmEnable2FAAsync(UserContext.UserId, dto);
            return Ok(new { Message = "Xác thực 2 bước đã được bật thành công." });
        }
    }
}
