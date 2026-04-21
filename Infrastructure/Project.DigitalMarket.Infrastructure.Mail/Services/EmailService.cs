using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Project.DigitalMarket.Domain.ExternalServices.Mails;
using Project.DigitalMarket.Domain.Share.Config;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Infrastructure.Mail.Services
{
    /// <summary>
    /// Thực thi (Implementation) việc gửi email qua SMTP (MailKit).
    /// Kế thừa và thực hiện contract IEmailManager từ Domain Layer.
    /// </summary>
    public class EmailService : IEmailDigitalMarketManager
    {
        private readonly ILazyloadProvider _lazyloadProvider;
        private IEmailConfig _emailConfig => _lazyloadProvider.LazyGetRequiredService<IEmailConfig>();

        public EmailService(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }

        public async Task SendEmailAsync(string[] toEmails, string subject, string body)
        {
            var email = new MimeMessage();
            if (_emailConfig.FromName.HasValue())
            {
                email.Sender = new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail);
            }
            else
            {
                email.Sender = MailboxAddress.Parse(_emailConfig.FromEmail);
            }

            foreach(var to in toEmails)
            {
                email.To.Add(MailboxAddress.Parse(to));
            }
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailConfig.SmtpHost, _emailConfig.SmtpPort, SecureSocketOptions.StartTlsWhenAvailable);
            await smtp.AuthenticateAsync(_emailConfig.SmtpUser, _emailConfig.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
