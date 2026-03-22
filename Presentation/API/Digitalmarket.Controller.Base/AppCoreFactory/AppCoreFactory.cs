using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Managers.Auths;
using Project.DigitalMarket.Application.Contract.Services.Auths;
using Project.DigitalMarket.Application.Contract.Services.Mails;
using Project.DigitalMarket.Application.Services.Auths;
using Project.DigitalMarket.Domain.ExternalServices.Mails;
using Project.DigitalMarket.Infrastructure.Mail.Services;
using Project.DigitalMarket.Infrastructure.MsSql.Data;
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
            services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()
                                 ?? context.Request.Headers["Authentication"].FirstOrDefault();

                        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = token.Substring("Bearer ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // 4. Đăng ký Services (Application Layer)
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, Project.DigitalMarket.Application.Services.Mails.EmailService>();

            // 5. Đăng ký External Services (Domain Contract <-> Infras Implementation)
            services.AddScoped<IEmailManager, EmailService>();

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
        /// DI cho domain logic (Manager)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseAppManagerFactory(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<Project.DigitalMarket.Domain.Repositories.Auths.IAuthRepository, Project.DigitalMarket.Infrastructure.MsSql.Repositories.Auths.AuthRepository>();
            return services;
        }
    }
}
