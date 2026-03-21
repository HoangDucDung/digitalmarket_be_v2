namespace Project.DigitalMarket.Domain.Entities
{
    /// <summary>
    /// Thông tin pháp lý/KYC cho người bán (Seller)
    /// </summary>
    public class UserKycProfileEntity
    {
        /// <summary>
        /// Khóa ngoại tham chiếu Users.Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Loại giấy tờ (IdentityCard, Passport...)
        /// </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Số giấy tờ định danh (Cần mã hóa khi lưu)
        /// </summary>
        public string DocumentNumber { get; set; } = string.Empty;

        /// <summary>
        /// Ảnh mặt trước giấy tờ
        /// </summary>
        public string? FrontImageUrl { get; set; }

        /// <summary>
        /// Ảnh mặt sau giấy tờ
        /// </summary>
        public string? BackImageUrl { get; set; }

        /// <summary>
        /// Mã số thuế (Nếu là Seller)
        /// </summary>
        public string? TaxId { get; set; }

        /// <summary>
        /// Trạng thái xác thực (Pending, Approved, Rejected)
        /// </summary>
        public string VerificationStatus { get; set; } = "Pending";

        /// <summary>
        /// Ghi chú từ người duyệt (Nếu bị từ chối)
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Ngày tạo hồ sơ KYC
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Ngày duyệt hồ sơ
        /// </summary>
        public DateTime? VerifiedAt { get; set; }

        /// <summary>
        /// Navigation property tới UserEntity
        /// </summary>
        public virtual UserEntity User { get; set; } = null!;
    }
}
