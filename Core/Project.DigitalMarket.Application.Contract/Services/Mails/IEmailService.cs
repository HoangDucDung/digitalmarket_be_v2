namespace Project.DigitalMarket.Application.Contract.Services.Mails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailAsync(string[] toEmails, string subject, string body);
    }
}
