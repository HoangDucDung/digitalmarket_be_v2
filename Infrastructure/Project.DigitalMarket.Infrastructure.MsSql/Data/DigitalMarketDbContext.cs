using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Infrastructure.MsSql.Data
{
    /// <summary>
    /// DbContext tích hợp Identity, kế thừa IdentityDbContext để tự động quản lý bảng User, Role, Claims...
    /// </summary>
    public class DigitalMarketDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
    {
        public DigitalMarketDbContext(DbContextOptions<DigitalMarketDbContext> options) : base(options)
        {
        }

        public DbSet<UserKycProfileEntity> UserKycProfiles { get; set; }
        public DbSet<UserFinancialTieEntity> UserFinancialTies { get; set; }
        public DbSet<UserAuditLogEntity> UserAuditLogs { get; set; }
        public DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình ApplicationUser
            builder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.FullName).HasMaxLength(256);
                entity.Property(u => u.AvatarUrl).HasMaxLength(500);
                entity.Property(u => u.Bio).HasMaxLength(1000);
                
                // Quan hệ 1-1 với KYC Profile (Seller)
                entity.HasOne(u => u.KycProfile)
                      .WithOne(k => k.User)
                      .HasForeignKey<UserKycProfileEntity>(k => k.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Quan hệ 1-N với Financial Ties (Billing/Payout)
                entity.HasMany(u => u.FinancialTies)
                      .WithOne(f => f.User)
                      .HasForeignKey(f => f.UserId);

                // Quan hệ 1-N với Audit Logs
                entity.HasMany(u => u.AuditLogs)
                      .WithOne(a => a.User)
                      .HasForeignKey(a => a.UserId);
            });

            // Cấu hình KYC Profile
            builder.Entity<UserKycProfileEntity>(entity =>
            {
                entity.ToTable("UserKycProfiles");
                entity.HasKey(k => k.UserId);
                
                entity.Property(k => k.DocumentType).HasMaxLength(50).IsRequired();
                entity.Property(k => k.DocumentNumber).HasMaxLength(100).IsRequired();
                entity.Property(k => k.VerificationStatus).HasMaxLength(20).IsRequired().HasDefaultValue("Pending");
                entity.Property(k => k.TaxId).HasMaxLength(50);
            });

            // Cấu hình Financial Ties
            builder.Entity<UserFinancialTieEntity>(entity =>
            {
                entity.ToTable("UserFinancialTies");
                entity.HasKey(f => f.Id);
                
                entity.Property(f => f.Type).HasMaxLength(50).IsRequired();
                entity.Property(f => f.Provider).HasMaxLength(100).IsRequired();
                entity.Property(f => f.AccountName).HasMaxLength(256).IsRequired();
                entity.Property(f => f.AccountNumber).HasMaxLength(256).IsRequired(); // Encrypted at app level
                
                entity.HasIndex(f => f.UserId);
            });

            // Cấu hình Audit Logs
            builder.Entity<UserAuditLogEntity>(entity =>
            {
                entity.ToTable("UserAuditLogs");
                entity.HasKey(a => a.Id);
                
                entity.Property(a => a.Action).HasMaxLength(100).IsRequired();
                entity.Property(a => a.IpAddress).HasMaxLength(50).IsRequired();
                entity.Property(a => a.UserAgent).HasMaxLength(500);
                
                entity.HasIndex(a => new { a.UserId, a.CreatedAt });
            });

            // Cấu hình Product cho feed khám phá
            builder.Entity<ProductEntity>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).HasMaxLength(512).IsRequired();
                entity.Property(p => p.Slug).HasMaxLength(512).IsRequired();
                entity.Property(p => p.ImageUrl).HasMaxLength(1000).IsRequired();
                entity.Property(p => p.ShopName).HasMaxLength(256).IsRequired();
                entity.Property(p => p.ShopLocation).HasMaxLength(256).IsRequired();
                entity.Property(p => p.Currency).HasMaxLength(10).IsRequired();
                entity.Property(p => p.CategoryBundle).HasMaxLength(100).IsRequired();
                entity.Property(p => p.Status).HasMaxLength(20).IsRequired();
                entity.Property(p => p.OriginalPrice).HasPrecision(18, 2);
                entity.Property(p => p.SalePrice).HasPrecision(18, 2);
                entity.Property(p => p.RatingAverage).HasPrecision(3, 2);

                entity.HasIndex(p => p.Slug).IsUnique();
                entity.HasIndex(p => new { p.SellerId, p.Status, p.IsDeleted });
                entity.HasIndex(p => new { p.IsDeleted, p.IsActive, p.Status, p.CategoryBundle });
            });
        }
    }
}
