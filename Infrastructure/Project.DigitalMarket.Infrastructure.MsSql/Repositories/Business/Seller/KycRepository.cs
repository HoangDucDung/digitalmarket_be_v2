using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Business.Seller;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Seller
{
    /// <summary>
    /// Triển khai Repository cho KYC Profile
    /// </summary>
    internal sealed class KycRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<UserKycProfileEntity>(lazyloadProvider), IKycRepository
    {
        public async Task<UserKycProfileEntity?> GetByUserIdAsync(Guid userId)
        {
            return await GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
