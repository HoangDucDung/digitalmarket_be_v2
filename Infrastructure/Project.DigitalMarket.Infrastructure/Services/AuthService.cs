using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Application.Contract.DTOs;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths;
using Project.DigitalMarket.Host.Base.Configs;
using Project.DigitalMarket.Libs.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Project.DigitalMarket.Domain.Services;

namespace Project.DigitalMarket.Infrastructure.Services
{
    /// <summary>
    /// Service xử lý đăng ký, đăng nhập và tạo JWT token
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly AuthConfig _authConfig;

        public AuthService(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IOptions<AuthConfig> authConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authConfig = authConfig.Value;
        }

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
        /// Tạo JWT token từ thông tin user
        /// </summary>
        private AuthResponseDto GenerateJwtToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(_authConfig.ExpiresTime);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty
            };
        }
    }
}
