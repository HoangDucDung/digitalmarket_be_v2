using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Domain.Share.Constants.Business;


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
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình CartItem
            builder.Entity<CartItemEntity>(entity =>
            {
                entity.ToTable("CartItems");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.ReferencePrice).HasPrecision(18, 2);

                entity.HasOne(c => c.Product)
                    .WithMany()
                    .HasForeignKey(c => c.ProductId);

                entity.HasIndex(c => c.UserId);
            });

            // Cấu hình Order
            builder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.OrderCode).HasMaxLength(50).IsRequired();
                entity.Property(o => o.Status).HasMaxLength(20).IsRequired();
                entity.Property(o => o.PaymentMethod).HasMaxLength(50).IsRequired();
                entity.Property(o => o.Subtotal).HasPrecision(18, 2);
                entity.Property(o => o.DiscountTotal).HasPrecision(18, 2);
                entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
                entity.Property(o => o.ProcessingFee).HasPrecision(18, 2);

                entity.HasIndex(o => o.OrderCode).IsUnique();
                entity.HasIndex(o => o.BuyerId);
            });

            // Cấu hình OrderItem
            builder.Entity<OrderItemEntity>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.ProductName).HasMaxLength(512).IsRequired();
                entity.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
                entity.Property(oi => oi.OriginalPrice).HasPrecision(18, 2);
                entity.Property(oi => oi.Subtotal).HasPrecision(18, 2);

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.Items)
                    .HasForeignKey(oi => oi.OrderId);

                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId);
            });

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

                // Cấu hình navigation property cho Roles (IdentityUserRole)
                entity.HasMany(u => u.UserRoles)
                      .WithOne()
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired();
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

            // Seeding Roles
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("B74DDD14-6340-4840-95C2-DB12554843E5"),
                    Name = RoleConstants.Customer,
                    NormalizedName = RoleConstants.Customer.ToUpper(),
                    ConcurrencyStamp = "e10a6f9b-7d9a-4f1a-b1c8-5a2c3d4e5f6a"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("69BD714F-9576-45BA-B5B7-F00649BE00DE"),
                    Name = RoleConstants.Seller,
                    NormalizedName = RoleConstants.Seller.ToUpper(),
                    ConcurrencyStamp = "d20b7f0c-8e0b-5a2b-c2d9-6b3d4e5f6a7b"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("8D04DCE2-969A-435D-BBA4-072895A5531B"),
                    Name = RoleConstants.Admin,
                    NormalizedName = RoleConstants.Admin.ToUpper(),
                    ConcurrencyStamp = "c30c8f1d-9f1c-6b3c-d3e0-7c4d5e6f7a8b"
                }
            );
        }
    }
}
