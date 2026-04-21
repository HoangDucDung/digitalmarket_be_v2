using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Models.Auths;
using Project.DigitalMarket.Domain.Share.Config;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.Extensions.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.DigitalMarket.Domain.Managers.Auths
{
    internal sealed class AuthManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IAuthManager
    {
        private IAuthConfig _authConfig => _lazyloadProvider.LazyGetRequiredService<IAuthConfig>();

        private UserManager<UserEntity> userManager => _lazyloadProvider.LazyGetRequiredService<UserManager<UserEntity>>();

        /// <summary>
        /// Tạo JWT token từ thông tin user
        /// </summary>
        public async Task<InfoToken> GenerateJwtToken(UserEntity user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = GenerateExtentions.Now.AddMinutes(_authConfig.ExpiresTime);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("IsCustomer", roles.Contains(RoleConstants.Customer).ToString().ToLower()),
                new Claim("IsSeller", roles.Contains(RoleConstants.Seller).ToString().ToLower()),
                new Claim("UserRoles", string.Join(",", roles)),
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
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList(),
                IsSeller = roles.Contains(RoleConstants.Seller)
            };
        }

        /// <summary>
        /// Tạo refresh token ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomNumber);
        //        return Convert.ToBase64String(randomNumber);
        //    }
        //}
    }
}
