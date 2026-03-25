namespace Project.DigitalMarket.Domain.Share.Constants.Business
{
    /// <summary>
    /// Các constant liên quan đến Đơn hàng
    /// </summary>
    public static class OrderConstants
    {
        public static class Status
        {
            /// <summary> Chờ thanh toán </summary>
            public const string Pending = "Pending";
            /// <summary> Đã thanh toán, chờ giao hàng/kích hoạt </summary>
            public const string Processing = "Processing";
            /// <summary> Đã hoàn thành </summary>
            public const string Completed = "Completed";
            /// <summary> Đã hủy </summary>
            public const string Cancelled = "Cancelled";
            /// <summary> Đã hoàn tiền </summary>
            public const string Refunded = "Refunded";
        }

        public static class PaymentMethod
        {
            /// <summary> Số dư tài khoản hệ thống </summary>
            public const string InternalBalance = "InternalBalance";
            /// <summary> Chuyển khoản ngân hàng </summary>
            public const string BankTransfer = "BankTransfer";
            /// <summary> Ví điện tử </summary>
            public const string EWallet = "EWallet";
        }
    }
}
