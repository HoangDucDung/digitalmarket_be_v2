using Microsoft.AspNetCore.Identity;
using Project.DigitalMarket.Application.Contract.DTOs.Auths;
using Project.DigitalMarket.Application.Contract.Services.Auths;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Managers.Auths;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.Extensions.Extensions;
using Microsoft.Extensions.Logging;

namespace Project.DigitalMarket.Application.Services.Auths
{
    /// <summary>
    /// Service xử lý đăng ký, đăng nhập và tạo JWT token.
    /// Chuyển lại Application theo yêu cầu (Business Logic layer).
    /// </summary>
    internal sealed class AuthService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<AuthService>(lazyloadProvider), IAuthService
    {
        private UserManager<UserEntity> _userManager => _lazyloadProvider.LazyGetRequiredService<UserManager<UserEntity>>();
        private SignInManager<UserEntity> _signInManager => _lazyloadProvider.LazyGetRequiredService<SignInManager<UserEntity>>();
        private IAuthManager _authManager => _lazyloadProvider.LazyGetRequiredService<IAuthManager>();
        //private IEmailService _emailService => _lazyloadProvider.LazyGetRequiredService<IEmailService>();

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogDebug("Bắt đầu đăng ký tài khoản mới với email: {Email}", registerDto.Email);
            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new BusinessException(ErrorCode.AccountAlreadyExists, "Email đã được sử dụng.");
            }

            var user = new UserEntity
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                CreatedAt = GenerateExtentions.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException(ErrorCode.RegistrationFailed, $"Đăng ký thất bại: {errors}");
            }

            // Gán role mặc định là Customer
            await _userManager.AddToRoleAsync(user, RoleConstants.Customer);

            // Gửi email xác thực (Tạm thời ẩn và ghi log console)
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            // await _emailService.SendEmailAsync(user.Email!, "Xác thực email", $"Mã xác thực của bạn là: {token}");
            Console.WriteLine($"\n[TEST LOG] Email: {user.Email} | Type: Xác thực email | Token: {token}\n");

            // Trả về response trống vì yêu cầu xác thực email trước khi đăng nhập
            return new AuthResponseDto { Email = user.Email! };
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new AuthException(ErrorCode.InvalidCredentials, "Email hoặc mật khẩu không đúng.");
            }

            if (!user.EmailConfirmed)
            {
                throw new AuthException(ErrorCode.EmailNotConfirmed, "Vui lòng xác thực email trước khi đăng nhập.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (result.RequiresTwoFactor)
            {
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                // await _emailService.SendEmailAsync(user.Email!, "Mã xác thực 2FA", $"Mã 2FA của bạn là: {token}");
                Console.WriteLine($"\n[TEST LOG] Email: {user.Email} | Type: 2FA Login | Code: {token}\n");
                return new AuthResponseDto { RequiresTwoFactor = true, Email = user.Email! };
            }

            if (!result.Succeeded)
            {
                throw new AuthException(ErrorCode.InvalidCredentials, "Email hoặc mật khẩu không đúng.");
            }

            return await GenerateJwtTokenAsync(user);
        }

        /// <summary>
        /// Tạo JWT token từ thông tin user thông qua Manager
        /// </summary>
        private async Task<AuthResponseDto> GenerateJwtTokenAsync(UserEntity user)
        {
            var infoToken = await _authManager.GenerateJwtToken(user);
            return _mapper.Map<AuthResponseDto>(infoToken);
        }

        public async Task VerifyEmailAsync(VerifyEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) throw new AuthException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại.");

            var result = await _userManager.ConfirmEmailAsync(user, dto.Token);
            if (!result.Succeeded) throw new AuthException(ErrorCode.InvalidToken, "Mã xác thực không hợp lệ or đã hết hạn.");
        }

        public async Task Enable2FAAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new AuthException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại.");

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            // await _emailService.SendEmailAsync(user.Email!, "Kích hoạt 2FA", $"Mã kích hoạt 2FA của bạn là: {token}");
            Console.WriteLine($"\n[TEST LOG] Email: {user.Email} | Type: Enable 2FA | Code: {token}\n");
        }

        public async Task ConfirmEnable2FAAsync(Guid userId, ConfirmEnable2FADto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new AuthException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại.");

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", dto.Code);
            if (!isValid) throw new AuthException(ErrorCode.InvalidToken, "Mã kích hoạt không hợp lệ.");

            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }

        public async Task<AuthResponseDto> Verify2FALoginAsync(Verify2FALoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) throw new AuthException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại.");

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", dto.Code);
            if (!isValid) throw new AuthException(ErrorCode.InvalidToken, "Mã 2FA không hợp lệ.");

            // Trực tiếp sinh JWT nếu mã 2FA đúng (vì bước trước đó đã verify Password thành công)
            return await GenerateJwtTokenAsync(user);
        }
    }
}
