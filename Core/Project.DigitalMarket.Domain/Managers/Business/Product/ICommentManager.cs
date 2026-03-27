using Project.DigitalMarket.Domain.Entities.Business;

namespace Project.DigitalMarket.Domain.Managers.Business.Product
{
    /// <summary>
    /// Interface quản lý logic nghiệp vụ domain cho Comment.
    /// </summary>
    public interface ICommentManager
    {
        /// <summary>
        /// Tạo mới một bình luận.
        /// </summary>
        Task<CommentEntity> CreateAsync(CommentEntity comment);

        /// <summary>
        /// Lấy danh sách bình luận của sản phẩm.
        /// </summary>
        Task<List<CommentEntity>> GetProductCommentsAsync(Guid productId);
    }
}
