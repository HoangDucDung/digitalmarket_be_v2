namespace Project.DigitalMarket.Libs.Constants.ErrorCode
{
    /// <summary>
    /// Mã lỗi liên quan đến xác thực (Authentication) và ủy quyền (Authorization) - 1000
    /// </summary>
    public partial class ErrorCode
    {
        /// <summary>
        /// Thông tin đăng nhập không chính xác
        /// </summary>
        public const int InvalidCredentials = 1000;

        /// <summary>
        /// Email chưa được xác thực
        /// </summary>
        public const int EmailNotConfirmed = 1001;

        /// <summary>
        /// Mã xác thực không hợp lệ hoặc đã hết hạn
        /// </summary>
        public const int InvalidToken = 1002;

        /// <summary>
        /// Tài khoản không tồn tại
        /// </summary>
        public const int AccountNotFound = 1003;

        /// <summary>
        /// Chưa đăng nhập hoặc Token không hợp lệ
        /// </summary>
        public const int Unauthorized = 1004;

        /// <summary>
        /// Không có quyền truy cập
        /// </summary>
        public const int Forbidden = 1005;
    }
}
