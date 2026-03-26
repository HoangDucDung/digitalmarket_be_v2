using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Domain.Managers.Business.Product
{
    /// <summary>
    /// Thực thi quản lý logic nghiệp vụ cho Comment.
    /// </summary>
    public class CommentManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), ICommentManager
    {
        private ICommentRepository _commentRepository => _lazyloadProvider.LazyGetRequiredService<ICommentRepository>();

        /// <summary>
        /// Tạo mới một bình luận.
        /// </summary>
        public async Task<CommentEntity> CreateAsync(CommentEntity comment)
        {
            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return comment;
        }

        /// <summary>
        /// Lấy danh sách bình luận của sản phẩm.
        /// </summary>
        public async Task<List<CommentEntity>> GetProductCommentsAsync(Guid productId)
        {
            return await _commentRepository.GetByProductId(productId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
