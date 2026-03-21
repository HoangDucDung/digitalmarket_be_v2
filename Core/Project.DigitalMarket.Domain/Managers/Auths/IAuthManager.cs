using Project.DigitalMarket.Domain.Models.Auths;

namespace Project.DigitalMarket.Domain.Managers.Auths
{
    public interface IAuthManager
    {
        /// <summary>
        /// Đăng nhập và trả về token thông tin người dùng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<InfoToken> SignInAsync(SignInRequest request);
    }
}
