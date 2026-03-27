using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Product
{
    public class ProductRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<ProductEntity>(lazyloadProvider), IProductRepository
    {
        public IQueryable<ProductEntity> GetDiscoverQuery()
        {
            var now = GenerateExtentions.Now;
            return GetByCondition(x =>
                !x.IsDeleted
                && x.IsActive
                && x.Status == ProductConstants.Status.Published
                && x.PublishedAt != null
                && x.PublishedAt <= now)
                .Include(x => x.Images)
                .Include(x => x.Variants).ThenInclude(v => v.Attributes)
                .Include(x => x.Rating)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Seller);
        }

        public IQueryable<CategoryEntity> GetCategoryTreeQuery(bool includeDisabled)
        {
            var query = _context.Set<CategoryEntity>().Where(x => !x.IsDeleted);
            if (!includeDisabled)
            {
                query = query.Where(x => x.IsActive);
            }

            return query;
        }

        public Task<CategoryEntity?> ResolveCategoryAsync(string categoryNameOrSlug)
        {
            var key = categoryNameOrSlug.Trim().ToLower();
            return _context.Set<CategoryEntity>()
                .Where(x => x.IsActive && !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Slug.ToLower() == key || x.Name.ToLower() == key);
        }

        public Task<BrandEntity?> ResolveBrandAsync(string? brandNameOrSlug)
        {
            if (string.IsNullOrWhiteSpace(brandNameOrSlug))
            {
                return Task.FromResult<BrandEntity?>(null);
            }

            var key = brandNameOrSlug.Trim().ToLower();
            return _context.Set<BrandEntity>()
                .Where(x => x.IsActive && !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Slug.ToLower() == key || x.Name.ToLower() == key);
        }
    }
}
