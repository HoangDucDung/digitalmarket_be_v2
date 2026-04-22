using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Product
{
    /// <summary>
    /// Interface repository cho thực thể Comment.
    /// </summary>
    public interface ICommentRepository : IRepositoryBase<CommentEntity>
    {
        /// <summary>
        /// Lấy danh sách bình luận của một sản phẩm.
        /// </summary>
        Task<List<CommentEntity>> GetByProductIdAsync(Guid productId);
    }
}
