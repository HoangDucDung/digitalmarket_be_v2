using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.Extensions.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// Thực thể Lưu trữ lịch sử giao dịch của ví điện tử
    /// </summary>
    public class WalletTransactionEntity
    {
        /// <summary>
        /// ID giao dịch (Khóa chính)
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ID ví liên quan (FK to Wallets.UserId)
        /// </summary>
        public Guid WalletId { get; set; }

        /// <summary>
        /// Số tiền giao dịch (Ví dụ: +1000 nạp, -500 thanh toán)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Loại giao dịch (Deposit, Withdrawal, Payment, Refund)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Trạng thái giao dịch (Pending, Completed, Failed, Cancelled)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Nội dung chi tiết giao dịch
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Mã tham chiếu ngoại vi (Ví dụ: OrderCode)
        /// </summary>
        public string? ReferenceId { get; set; }

        /// <summary>
        /// Thời điểm phát sinh giao dịch
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        // Navigation
        [ForeignKey("WalletId")]
        public virtual WalletEntity Wallet { get; set; }
    }
}
