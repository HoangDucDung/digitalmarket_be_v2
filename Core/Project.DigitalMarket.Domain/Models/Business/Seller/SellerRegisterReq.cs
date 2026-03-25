namespace Project.DigitalMarket.Domain.Models.Business.Seller
{
    /// <summary>
    /// Request model cho nghiệp vụ đăng ký bán hàng (Domain layer)
    /// </summary>
    public class SellerRegisterReq
    {
        #region Thông tin KYC
        /// <summary>
        /// Loại giấy tờ định danh (vd: CCCD, Passport)
        /// </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Số giấy tờ định danh
        /// </summary>
        public string DocumentNumber { get; set; } = string.Empty;

        /// <summary>
        /// URL ảnh mặt trước của giấy tờ
        /// </summary>
        public string? FrontImageUrl { get; set; }

        /// <summary>
        /// URL ảnh mặt sau của giấy tờ
        /// </summary>
        public string? BackImageUrl { get; set; }

        /// <summary>
        /// Mã số thuế (nếu có)
        /// </summary>
        public string? TaxId { get; set; }
        #endregion

        #region Thông tin tài chính
        /// <summary>
        /// Phương thức nhận tiền (vd: BankAccount, PayPal)
        /// </summary>
        public string PayoutType { get; set; } = string.Empty;

        /// <summary>
        /// Nhà cung cấp (Tên ngân hàng hoặc nền tảng)
        /// </summary>
        public string PayoutProvider { get; set; } = string.Empty;

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string PayoutAccountName { get; set; } = string.Empty;

        /// <summary>
        /// Số tài khoản nhận tiền
        /// </summary>
        public string PayoutAccountNumber { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Tiểu sử/Giới thiệu của người bán
        /// </summary>
        public string? Bio { get; set; }
    }
}
