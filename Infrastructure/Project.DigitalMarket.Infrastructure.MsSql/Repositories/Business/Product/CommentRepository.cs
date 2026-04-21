using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Product
{
    /// <summary>
    /// Thực thi repository cho Comment sử dụng Entity Framework Core.
    /// </summary>
    internal sealed class CommentRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<CommentEntity>(lazyloadProvider), ICommentRepository
    {
        public IQueryable<CommentEntity> GetByProductId(Guid productId)
        {
            return _dbSet.Where(x => x.ProductId == productId && !x.IsDeleted);
        }
    }
}
