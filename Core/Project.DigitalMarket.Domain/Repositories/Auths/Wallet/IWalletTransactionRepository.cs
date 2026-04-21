using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Auths.Wallet
{
    public interface IWalletTransactionRepository : IRepositoryBase<WalletTransactionEntity>
    {
        /// <summary>
        /// Lấy danh sách lịch sử giao dịch của người dùng có phân trang
        /// </summary>
        Task<List<WalletTransactionEntity>> GetPagedByUserIdAsync(Guid userId, int page, int pageSize);
    }
}
