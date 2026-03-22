namespace Project.DigitalMarket.Application.Contract.DTOs.Auths
{
    public class Verify2FALoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
