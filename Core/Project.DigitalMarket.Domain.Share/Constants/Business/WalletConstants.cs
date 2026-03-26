namespace Project.DigitalMarket.Domain.Share.Constants.Business
{
    /// <summary>
    /// Các hằng số định nghĩa cho mô hình Ví (Wallet)
    /// </summary>
    public static class WalletConstants
    {
        public static class TransactionType
        {
            public const string Deposit = "Deposit";      // Nạp tiền
            public const string Withdrawal = "Withdrawal"; // Rút tiền
            public const string Payment = "Payment";      // Thanh toán
            public const string Refund = "Refund";        // Hoàn tiền
        }

        public static class TransactionStatus
        {
            public const string Pending = "Pending";     // Đang chờ
            public const string Completed = "Completed"; // Thành công
            public const string Failed = "Failed";       // Thất bại
            public const string Cancelled = "Cancelled"; // Đã hủy
        }

        public static class WalletStatus
        {
            public const string Active = "Active"; // Hoạt động
            public const string Locked = "Locked"; // Bị khóa
        }
    }
}
