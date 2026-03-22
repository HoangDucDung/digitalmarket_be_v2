using Project.DigitalMarket.Application.Contract.DTOs.Auths;

namespace Project.DigitalMarket.Application.Contract.Services.Auths
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task VerifyEmailAsync(VerifyEmailDto dto);
        Task Enable2FAAsync(Guid userId);
        Task ConfirmEnable2FAAsync(Guid userId, ConfirmEnable2FADto dto);
        Task<AuthResponseDto> Verify2FALoginAsync(Verify2FALoginDto dto);
    }
}
