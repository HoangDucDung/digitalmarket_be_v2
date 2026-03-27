using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Product
{
    public interface ICommentService
    {
        Task<ApiResponse<CommentResDto>> CreateCommentAsync(CreateCommentReqDto request);
        Task<ApiResponse<List<CommentResDto>>> GetProductCommentsAsync(Guid productId);
    }
}
