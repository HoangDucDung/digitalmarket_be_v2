using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Interfaces;
using Project.DigitalMarket.Host.Base.Configs;
using Project.DigitalMarket.Infrastructure.Data;
using Project.DigitalMarket.Infrastructure.Services;
using Project.DigitalMarket.Libs.DependencyInjection;
using System.Text;

namespace Digitalmarket.Controller.Base.AppCoreFactory
{
    /// <summary>
    /// Class DI factory để tạo các instance của các lớp trong AppCore, giúp tách rời việc khởi tạo và quản lý các đối tượng, đồng thời hỗ trợ việc mở rộng và bảo trì mã nguồn dễ dàng hơn.
    /// </summary>
    public static class AppCoreFactory
    {
        public static IServiceCollection AddLazyloadFactory(this IServiceCollection service)
        {
            service.AddScoped<ICachedServiceProviderBase, CachedServiceProviderBase>();
            service.AddScoped<ILazyloadProvider, LazyloadProvider>();
            return service;
        }

        /// <summary>
        /// DI cho auth: Đăng ký Identity, JWT Authentication, DbContext, AuthService
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">Configuration để đọc AuthConfig và ConnectionString</param>
        /// <returns></returns>
        public static IServiceCollection UseAppAuthenFactory(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Đăng ký DbContext với SQL Server
            var connectionString = configuration.GetSection("ConnectionString:SqlServer").Value;
            services.AddDbContext<DigitalMarketDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. Đăng ký Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DigitalMarketDbContext>()
            .AddDefaultTokenProviders();

            // 3. Cấu hình JWT Authentication
            var authConfig = configuration.GetSection("AuthConfig");
            var secretKey = authConfig["SecretKey"];

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
                    ValidIssuer = authConfig["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = authConfig["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // 4. Đăng ký AuthService
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        /// <summary>
        /// DI cho bussiness logic
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseAppBussinessFactory(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// DI cho domain logic
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseAppDomainFactory(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// DI cho domain logic
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseAppManagerFactory(this IServiceCollection services)
        {
            return services;
        }
    }
}

