using Project.DigitalMarket.Domain.Entities;

namespace Project.DigitalMarket.Domain.Repositories.Auths
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="registerDto">Thông tin đăng ký</param>
        /// <returns>Kết quả đăng ký</returns>
        Task<bool> RegisterAsync(UserEntity registerDto);

        /// <summary>
        /// Lấy thông tin user theo userId
        /// </summary>
        /// <param name="userId">Id của user</param>
        /// <returns>Thông tin user</returns>
        Task<UserEntity?> GetUserByUserIdAsync(Guid userId);

        /// <summary>
        /// Lấy thông tin user theo email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Thông tin user</returns>
        Task<UserEntity?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Lưu refresh token cho user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<bool> SaveRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
