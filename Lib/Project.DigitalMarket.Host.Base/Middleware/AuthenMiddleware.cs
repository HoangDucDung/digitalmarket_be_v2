using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Host.Base.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Project.DigitalMarket.Host.Base.Middleware
{
    public class AuthenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await HandleAuthenToken(context).ConfigureAwait(false);
        }

        /// <summary>
        /// Xử lý token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Task HandleAuthenToken(HttpContext context)
        {
            try
            {
                HandleValidateToken(context);
                return _next(context);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Token is null.", ex);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                throw new Exception("Invalid signature.", ex);
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw new Exception("Expired token.", ex);
            }
            catch (SecurityTokenException ex)
            {
                throw new Exception("Token invalid.", ex);
            }
        }

        /// <summary>
        /// Hàm validate token
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        private void HandleValidateToken(HttpContext context)
        {
            // Lấy endpoint hiện tại
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                return;
            }

            // Nếu endpoint có attribute [AllowAnonymous] => bỏ qua
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                return;
            }

            var token = context.Request.Headers["Authentication"];
            var authConfig = context.RequestServices.GetService<IOptions<AuthConfig>>();

            if (authConfig == null) throw new Exception("AuthConfig is not configured.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authConfig.Value.SecretKey);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }
    }
}
