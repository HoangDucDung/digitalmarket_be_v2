namespace Project.DigitalMarket.Domain.Share.Config
{
    public interface IElasticConfig
    {
        /// <summary>
        /// Đường dẫn
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }
    }
}
