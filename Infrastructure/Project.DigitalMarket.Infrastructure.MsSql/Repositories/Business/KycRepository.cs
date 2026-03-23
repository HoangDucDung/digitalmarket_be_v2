using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business
{
    /// <summary>
    /// Triển khai Repository cho KYC Profile
    /// </summary>
    public class KycRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<UserKycProfileEntity>(lazyloadProvider), IKycRepository
    {
    }
}
