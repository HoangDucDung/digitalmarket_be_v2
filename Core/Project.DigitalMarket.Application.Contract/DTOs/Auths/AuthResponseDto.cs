namespace Project.DigitalMarket.Application.Contract.DTOs.Auths
{
    /// <summary>
    /// DTO cho response trả về sau khi đăng nhập thành công
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT Access Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian hết hạn của token
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Họ tên người dùng
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email người dùng
        /// </summary>
        public string Email { get; set; } = string.Empty;
        public bool RequiresTwoFactor { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsSeller { get; set; }
    }
}
