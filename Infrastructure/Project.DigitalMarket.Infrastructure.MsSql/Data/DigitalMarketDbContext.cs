using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;

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
        }
    }
}
