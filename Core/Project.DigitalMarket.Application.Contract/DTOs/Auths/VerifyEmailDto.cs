namespace Project.DigitalMarket.Application.Contract.DTOs.Auths
{
    public class VerifyEmailDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
