using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths.Wallet;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Wallet
{
    internal sealed class WalletTransactionRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<WalletTransactionEntity>(lazyloadProvider), IWalletTransactionRepository
    {
        public Task<List<WalletTransactionEntity>> GetPagedByUserIdAsync(Guid userId, int page, int pageSize)
        {
            return GetByCondition(x => x.WalletId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
