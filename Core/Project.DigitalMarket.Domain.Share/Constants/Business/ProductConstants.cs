namespace Project.DigitalMarket.Domain.Share.Constants.Business
{
    /// <summary>
    /// Các constant liên quan đến Sản phẩm
    /// </summary>
    public static class ProductConstants
    {
        public static class Status
        {
            /// <summary> Bản nháp, chưa công khai </summary>
            public const string Draft = "Draft";
            /// <summary> Đang hoạt động, hiển thị trên feed </summary>
            public const string Active = "Active";
            /// <summary> Đã bị ẩn bởi người bán </summary>
            public const string Hidden = "Hidden";
            /// <summary> Đã bị khóa bởi hệ thống </summary>
            public const string Banned = "Banned";
        }
    }
}
