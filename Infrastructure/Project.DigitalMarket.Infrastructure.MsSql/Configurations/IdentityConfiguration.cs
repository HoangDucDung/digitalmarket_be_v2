using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Infrastructure.MsSql.Data;

namespace Project.DigitalMarket.Infrastructure.MsSql.Configurations
{
    public static class IdentityConfiguration
    {
        /// <summary>
        /// Đăng ký cấu hình sử dụng Identity với SQL Server, bao gồm DbContext và các tùy chỉnh về password, user, v.v.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddMsSqlIdentity(this IServiceCollection services, IConfiguration configuration)
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
        }
    }
}
