using Microsoft.AspNetCore.Identity;
using Project.DigitalMarket.Application.Contract.DTOs;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Managers.Auths;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Domain.Services;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services
{
    /// <summary>
    /// Service xử lý đăng ký, đăng nhập và tạo JWT token.
    /// Chuyển từ Infrastructure sang Application theo kiến trúc Clean Architecture.
    /// </summary>
    public class AuthService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IAuthService
    {
        private UserManager<UserEntity> _userManager => _lazyloadProvider.GetRequiredService<UserManager<UserEntity>>();
        private SignInManager<UserEntity> _signInManager => _lazyloadProvider.GetRequiredService<SignInManager<UserEntity>>();
        private IAuthManager _authManager => _lazyloadProvider.GetRequiredService<IAuthManager>();

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new BusinessException("Email đã được sử dụng.");
            }

            var user = new UserEntity
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Đăng ký thất bại: {errors}");
            }

            // Tạo token sau khi đăng ký thành công
            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new AuthException("Email hoặc mật khẩu không đúng.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new AuthException("Email hoặc mật khẩu không đúng.");
            }

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Tạo JWT token từ thông tin user thông qua Manager
        /// </summary>
        private AuthResponseDto GenerateJwtToken(UserEntity user)
        {
            var infoToken = _authManager.GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = infoToken.Token,
                Expiration = infoToken.Expiration,
                FullName = infoToken.FullName,
                Email = infoToken.Email
            };
        }
    }
}
