using System.ComponentModel.DataAnnotations;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Seller
{
    /// <summary>
    /// DTO cho request đăng ký bán hàng (Seller Registration)
    /// </summary>
    public class SellerRegisterDto
    {
        #region Thông tin KYC (Pháp lý)
        /// <summary>
        /// Loại giấy tờ định danh (vd: CCCD, Passport, CMND)
        /// </summary>
        [Required(ErrorMessage = "Loại giấy tờ là bắt buộc")]
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Số giấy tờ định danh (vd: 00123...)
        /// </summary>
        [Required(ErrorMessage = "Số giấy tờ định danh là bắt buộc")]
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

        #region Thông tin tài chính (Nhận tiền)
        /// <summary>
        /// Loại thanh toán (vd: Payout_BankAccount, Payout_Paypal)
        /// </summary>
        [Required(ErrorMessage = "Loại tài khoản thanh toán là bắt buộc")]
        public string PayoutType { get; set; } = string.Empty;

        /// <summary>
        /// Nhà cung cấp dịch vụ (vd: Vietcombank, Stripe, PayPal)
        /// </summary>
        [Required(ErrorMessage = "Nhà cung cấp là bắt buộc")]
        public string PayoutProvider { get; set; } = string.Empty;

        /// <summary>
        /// Tên chủ tài khoản thanh toán
        /// </summary>
        [Required(ErrorMessage = "Tên chủ tài khoản là bắt buộc")]
        public string PayoutAccountName { get; set; } = string.Empty;

        /// <summary>
        /// Số tài khoản hoặc Email PayPal
        /// </summary>
        [Required(ErrorMessage = "Số tài khoản là bắt buộc")]
        public string PayoutAccountNumber { get; set; } = string.Empty;
        #endregion
        
        /// <summary>
        /// Giới thiệu bản thân hoặc thông tin cửa hàng
        /// </summary>
        public string? Bio { get; set; }
    }
}
