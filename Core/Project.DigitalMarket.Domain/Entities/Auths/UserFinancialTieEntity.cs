using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// Thông tin thanh toán (Billing) hoặc nhận tiền (Payout)
    /// </summary>
    public class UserFinancialTieEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Khóa ngoại tham chiếu Users.Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Loại: Payout_BankAccount, Payout_Paypal, Billing_CreditCard
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhà cung cấp (VD: Vietcombank, Stripe, PayPal)
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Số tài khoản (Cần mã hóa khi lưu)
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Đánh dấu là phương thức mặc định
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Ngày thêm thông tin
        /// </summary>
        public DateTime CreatedAt { get; set; } = GenerateExtentions.Now;

        /// <summary>
        /// Navigation property tới UserEntity
        /// </summary>
        public virtual UserEntity User { get; set; } = null!;
    }
}
