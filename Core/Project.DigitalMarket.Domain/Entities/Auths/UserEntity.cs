using Microsoft.AspNetCore.Identity;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.Extensions.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// User entity kế thừa IdentityUser, bổ sung thông tin tùy chỉnh
    /// </summary>
    public class UserEntity : IdentityUser<Guid>
    {
        /// <summary>
        /// Danh sách Role của User (Sử dụng entity IdentityUserRole của Identity)
        /// </summary>
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();

        /// <summary>
        /// Kiểm tra xem User có phải là Người dùng (Customer) không
        /// </summary>
        [NotMapped]
        public bool IsCustomer => UserRoles != null && UserRoles.Any(); // Tạm thời để Any, logic check role name nên dùng UserManager

        /// <summary>
        /// Kiểm tra xem User có phải là Người bán (Seller) không
        /// </summary>
        [NotMapped]
        public bool IsSeller => UserRoles != null && UserRoles.Any(); // Tạm thời để Any, logic check role name nên dùng UserManager

        /// <summary>
        /// Họ và tên đầy đủ
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Đường dẫn ảnh đại diện
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Giới thiệu bản thân (Dành cho Seller profile)
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Ngày tạo tài khoản
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        /// <summary>
        /// Ngày cập nhật thông tin gần nhất
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Thông tin KYC/Pháp lý (1-1)
        /// </summary>
        public virtual UserKycProfileEntity? KycProfile { get; set; }

        /// <summary>
        /// Danh sách tài khoản thanh toán/rút tiền (1-N)
        /// </summary>
        public virtual ICollection<UserFinancialTieEntity> FinancialTies { get; set; } = new List<UserFinancialTieEntity>();

        /// <summary>
        /// Danh sách nhật ký hoạt động quan trọng (1-N)
        /// </summary>
        public virtual ICollection<UserAuditLogEntity> AuditLogs { get; set; } = new List<UserAuditLogEntity>();
    }
}
