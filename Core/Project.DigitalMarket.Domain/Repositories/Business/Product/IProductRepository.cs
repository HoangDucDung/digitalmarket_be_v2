using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Product
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        IQueryable<ProductEntity> GetDiscoverQuery();
        IQueryable<CategoryEntity> GetCategoryTreeQuery(bool includeDisabled);
        Task<CategoryEntity?> ResolveCategoryAsync(string categoryNameOrSlug);
        Task<BrandEntity?> ResolveBrandAsync(string? brandNameOrSlug);
    }
}
