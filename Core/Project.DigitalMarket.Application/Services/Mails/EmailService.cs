using Project.DigitalMarket.Application.Contract.Services.Mails;
using Project.DigitalMarket.Domain.ExternalServices.Mails;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Mails
{
    /// <summary>
    /// Service logic xử lý gửi Email ở tầng Application.
    /// Gọi (Call) IEmailManager từ Domain Layer.
    /// </summary>
    internal sealed class EmailService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<EmailService>(lazyloadProvider), IEmailService
    {
        private IEmailDigitalMarketManager _emailManager => _lazyloadProvider.LazyGetRequiredService<IEmailDigitalMarketManager>();

        /// <summary>
        /// Gửi Email đến 1 địa chỉ
        /// </summary>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(new[] { to }, subject, body);
        }

        /// <summary>
        /// Gửi Email đến danh sách nhiều địa chỉ
        /// </summary>
        public async Task SendEmailAsync(string[] toEmails, string subject, string body)
        {
            await _emailManager.SendEmailAsync(toEmails, subject, body);
        }
    }
}
