using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Seller
{
    /// <summary>
    /// Repository cho các nghiệp vụ liên quan đến Financial Tie (Payout/Billing)
    /// </summary>
    public interface IFinancialRepository : IRepositoryBase<UserFinancialTieEntity>
    {
    }
}
