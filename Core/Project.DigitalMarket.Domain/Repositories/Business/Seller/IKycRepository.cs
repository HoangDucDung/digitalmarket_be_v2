using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Seller
{
    /// <summary>
    /// Repository cho các nghiệp vụ liên quan đến KYC Profile
    /// </summary>
    public interface IKycRepository : IRepositoryBase<UserKycProfileEntity>
    {
        /// <summary>
        /// Lấy thông tin KYC của người dùng
        /// </summary>
        Task<UserKycProfileEntity?> GetByUserIdAsync(Guid userId);
    }
}
