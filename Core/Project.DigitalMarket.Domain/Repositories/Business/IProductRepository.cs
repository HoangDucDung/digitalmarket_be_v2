using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        IQueryable<ProductEntity> GetDiscoverQuery();
    }
}
