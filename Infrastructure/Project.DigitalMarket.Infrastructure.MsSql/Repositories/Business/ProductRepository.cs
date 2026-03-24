using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business
{
    public class ProductRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<ProductEntity>(lazyloadProvider), IProductRepository
    {
        public IQueryable<ProductEntity> GetDiscoverQuery()
        {
            var now = GenerateExtentions.Now;
            return GetByCondition(x =>
                !x.IsDeleted
                && x.IsActive
                && x.Status == ProductStatus.Active
                && x.PublishedAt != null
                && x.PublishedAt <= now);
        }
    }
}
