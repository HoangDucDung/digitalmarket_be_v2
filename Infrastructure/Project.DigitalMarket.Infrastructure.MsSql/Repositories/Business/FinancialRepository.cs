using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business
{
    /// <summary>
    /// Triển khai Repository cho Financial Tie
    /// </summary>
    public class FinancialRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<UserFinancialTieEntity>(lazyloadProvider), IFinancialRepository
    {
    }
}
