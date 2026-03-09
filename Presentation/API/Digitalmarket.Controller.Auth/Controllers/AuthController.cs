using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Domain.DTOs;
using Project.DigitalMarket.Domain.Interfaces;

namespace Digitalmarket.Controller.Auth.Controllers
{
    /// <summary>
    /// Controller xử lý xác thực: đăng ký và đăng nhập
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

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
    }
}
