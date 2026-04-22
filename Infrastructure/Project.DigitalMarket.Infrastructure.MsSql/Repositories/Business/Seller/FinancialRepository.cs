using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Business.Seller;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Seller
{
    /// <summary>
    /// Triển khai Repository cho Financial Tie
    /// </summary>
    internal sealed class FinancialRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<UserFinancialTieEntity>(lazyloadProvider), IFinancialRepository
    {
        public async Task<UserFinancialTieEntity?> GetDefaultByUserIdAsync(Guid userId)
        {
            return await GetByCondition(x => x.UserId == userId && x.IsDefault).FirstOrDefaultAsync();
        }
    }
}
