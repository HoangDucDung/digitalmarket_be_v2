using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.Extensions.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// Thực thể Ví điện tử của người dùng
    /// </summary>
    public class WalletEntity
    {
        /// <summary>
        /// ID người dùng sở hữu ví (Đồng thời là Khóa chính)
        /// </summary>
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// Số dư hiện tại của ví
        /// </summary>
        public decimal Balance { get; set; } = 0;

        /// <summary>
        /// Trạng thái ví (Active/Locked)
        /// </summary>
        public string Status { get; set; } = WalletConstants.WalletStatus.Active;

        /// <summary>
        /// Thời điểm tạo ví
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        /// <summary>
        /// Thời điểm cập nhật ví lần cuối
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Danh sách lịch sử giao dịch của ví
        /// </summary>
        public virtual ICollection<WalletTransactionEntity> Transactions { get; set; } = new List<WalletTransactionEntity>();
    }
}
