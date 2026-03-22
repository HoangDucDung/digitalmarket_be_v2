using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Models.Auths;
using Project.DigitalMarket.Domain.Repositories.Auths;
using Project.DigitalMarket.Domain.Share.Config;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.Extensions.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.DigitalMarket.Domain.Managers.Auths
{
    public class AuthManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IAuthManager
    {
        private IAuthRepository _authRepository => _lazyloadProvider.LazyGetRequiredService<IAuthRepository>();
        private IAuthConfig _authConfig => _lazyloadProvider.LazyGetRequiredService<IAuthConfig>();

        public async Task<InfoToken> SignInAsync(SignInRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
                throw new BusinessException("User not found");

            var isPasswordValid = VerifyHashedPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
                throw new BusinessException("Invalid password");

            var infoToken = GenerateJwtToken(user);
            infoToken.RefreshToken = GenerateRefreshToken();

            // Lưu refresh token vào database hoặc cache nếu cần thiết
            await _authRepository.SaveRefreshTokenAsync(user.Id, infoToken.RefreshToken);

            return infoToken;
        }

        /// <summary>
        /// Kiểm tra mật khẩu đã được hash với mật khẩu gốc
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private bool VerifyHashedPassword(string password, string passwordHash)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null!, passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        /// <summary>
        /// Tạo JWT token từ thông tin user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public InfoToken GenerateJwtToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = GenerateExtentions.Now.AddMinutes(_authConfig.ExpiresTime);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new InfoToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty
            };
        }

        /// <summary>
        /// Tạo refresh token ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
