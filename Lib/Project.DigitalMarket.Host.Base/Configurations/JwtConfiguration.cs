using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Host.Base.Configs;
using Project.DigitalMarket.Libs.Exceptions;
using Project.Extensions.Extensions;
using System.Text;

namespace Project.DigitalMarket.Host.Base.Configurations
{
    public static class JwtConfiguration
    {
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình JWT Authentication
            var authConfig = configuration.GetSection(ConfigKeyConstant.AuthConfig).Get<AuthConfig>();

            if(authConfig == null) throw new AuthException("AuthConfig không tồn tại trong cấu hình.");

            var secretKey = authConfig.SecretKey;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()
                                 ?? context.Request.Headers["Authentication"].FirstOrDefault()
                                 ?? string.Empty;

                        if (token.HasValue() && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = token.Substring("Bearer ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
