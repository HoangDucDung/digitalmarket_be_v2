using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Wallet
{
    public interface IWalletRepository : IRepositoryBase<WalletEntity>
    {
        /// <summary>
        /// Lấy thông tin ví của người dùng
        /// </summary>
        Task<WalletEntity?> GetByUserIdAsync(Guid userId);
    }
}
