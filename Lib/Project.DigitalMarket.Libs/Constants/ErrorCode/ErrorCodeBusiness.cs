namespace Project.DigitalMarket.Libs.Constants.ErrorCode
{
    /// <summary>
    /// Mã lỗi nghiệp vụ (3000)
    /// </summary>
    public partial class ErrorCode
    {
        /// <summary>
        /// Tài khoản đã tồn tại (ví dụ: email đã được sử dụng)
        /// </summary>
        public const int AccountAlreadyExists = 3001;

        /// <summary>
        /// Đăng ký tài khoản thất bại
        /// </summary>
        public const int RegistrationFailed = 3002;

        /// <summary>
        /// Sản phẩm không khả dụng (hết hàng, bị ẩn hoặc xóa)
        /// </summary>
        public const int ProductNotAvailable = 3003;

        /// <summary>
        /// Đơn hàng không tồn tại
        /// </summary>
        public const int OrderNotFound = 3004;

        /// <summary>
        /// Giỏ hàng trống
        /// </summary>
        public const int EmptyCart = 3005;

        /// <summary>
        /// Chỉ có thể thực hiện thao tác trên đơn hàng đang chờ (Pending)
        /// </summary>
        public const int OnlyPendingOrderAllowed = 3006;

        /// <summary>
        /// Mục trong giỏ hàng không tồn tại
        /// </summary>
        public const int CartItemNotFound = 3007;

        /// <summary>
        /// Cấu hình (Configuration Section) không tồn tại
        /// </summary>
        public const int ConfigSectionNotFound = 3008;

        /// <summary>
        /// Không được phép mua sản phẩm của chính mình
        /// </summary>
        public const int CannotBuyOwnProduct = 3009;


        public const int InvalidProductData = 3010;
        public const int SlugAlreadyExists = 3011;
        public const int ProductNotFound = 3012;
    }
}
