namespace Project.DigitalMarket.Domain.Share.Config
{
    /// <summary>
    /// Interface cấu hình Email cho Layer Domain dùng chung
    /// </summary>
    public interface IEmailConfig
    {
        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpUser { get; }
        string SmtpPass { get; }
        string FromEmail { get; }
        string FromName { get; }
    }
}
