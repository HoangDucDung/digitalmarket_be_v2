using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;

namespace Project.DigitalMarket.Infrastructure.Data
{
    /// <summary>
    /// DbContext tích hợp Identity, kế thừa IdentityDbContext để tự động quản lý bảng User, Role, Claims...
    /// </summary>
    public class DigitalMarketDbContext : IdentityDbContext<ApplicationUser>
    {
        public DigitalMarketDbContext(DbContextOptions<DigitalMarketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình thêm cho ApplicationUser
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName).HasMaxLength(256);
            });
        }
    }
}
