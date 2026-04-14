using Project.DigitalMarket.Domain.Share.Config;

namespace Project.DigitalMarket.Host.Base.Configs
{
    public class ElasticConfig : IElasticConfig
    {
        /// <summary>
        /// Đường dẫn
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
