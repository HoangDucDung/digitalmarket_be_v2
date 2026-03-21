using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Models.Auths;
using Project.DigitalMarket.Domain.Repositories.Auths;
using Project.DigitalMarket.Domain.Share.Config;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.DigitalMarket.Domain.Managers.Auths
{
    public class AuthManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IAuthManager
    {
        private IAuthRepository _authRepository => _lazyloadProvider.GetRequiredService<IAuthRepository>();
        private IAuthConfig _authConfig => _lazyloadProvider.GetRequiredService<IAuthConfig>();

        public async Task<InfoToken> SignInAsync(SignInRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if(user == null) 
                throw new BusinessException("User not found");

            var isPasswordValid = VerifyHashedPassword(request.Password, request.Password);

            if(!isPasswordValid) 
                throw new BusinessException("Invalid password");

            var token = GenerateToken(user);

            // Lưu refresh token vào database hoặc cache nếu cần thiết
            await _authRepository.SaveRefreshTokenAsync(user.Id, token.RefreshToken);

            return token;
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
        /// Tạo JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private InfoToken GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_authConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(AppClaimTypes.UserId, user.Id.ToString()),
                    new Claim(AppClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(AppClaimTypes.Role, user.Role)
                ]),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new InfoToken
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = GenerateRefreshToken()
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
