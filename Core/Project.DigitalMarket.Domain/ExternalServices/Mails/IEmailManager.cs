namespace Project.DigitalMarket.Domain.ExternalServices.Mails
{
    /// <summary>
    /// Contract giao tiếp với các dịch vụ gửi email ngoại vi (Manager layer in Domain).
    /// Infrastructure sẽ kế thừa và thực thi.
    /// </summary>
    public interface IEmailManager
    {
        Task SendEmailAsync(string[] toEmails, string subject, string body);
    }
}
