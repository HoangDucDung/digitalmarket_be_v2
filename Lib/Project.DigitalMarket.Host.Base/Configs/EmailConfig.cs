using Project.DigitalMarket.Domain.Share.Config;

namespace Project.DigitalMarket.Host.Base.Configs
{
    /// <summary>
    /// File record dùng để map với cấu trúc thông số Email cấu hình ở Email.json
    /// </summary>
    public class EmailConfig : IEmailConfig
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPass { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
