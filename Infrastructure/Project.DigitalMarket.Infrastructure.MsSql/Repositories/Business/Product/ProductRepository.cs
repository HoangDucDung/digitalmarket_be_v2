using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Product
{
    internal sealed class ProductRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<ProductEntity>(lazyloadProvider), IProductRepository
    {
        public async Task<(List<ProductEntity> Items, int Total)> GetPagedDiscoveryAsync(int limit, int offset, string? keyword)
        {
            var query = GetDiscoverQuery();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.Trim();
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{k}%"));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.PublishedAt)
                .ThenByDescending(x => x.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return (items, total);
        }

        public Task<ProductEntity?> GetProductDetailByIdAsync(Guid productId)
        {
            return GetDiscoverQuery()
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public Task<List<CategoryEntity>> GetCategoryTreeAsync(bool includeDisabled)
        {
            return GetCategoryTreeQuery(includeDisabled)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public Task<bool> IsSlugExistsAsync(string slug)
        {
            return GetByCondition(x => x.Slug == slug).AnyAsync();
        }

        private IQueryable<ProductEntity> GetDiscoverQuery()
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

        private IQueryable<CategoryEntity> GetCategoryTreeQuery(bool includeDisabled)
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

        public async Task<ProductEntity?> GetActiveWithVariantsByIdAsync(Guid productId)
        {
            return await GetByCondition(x => x.Id == productId && x.IsActive && !x.IsDeleted)
                .Include(x => x.Variants)
                .FirstOrDefaultAsync();
        }
    }
}
