using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Managers.Business.Product;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;

namespace Project.DigitalMarket.Application.Services.Business.Product
{
    internal sealed class CommentService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<CommentService>(lazyloadProvider), ICommentService
    {
        private ICommentManager _commentManager => _lazyloadProvider.LazyGetRequiredService<ICommentManager>();

        public async Task<ApiResponse<CommentResDto>> CreateCommentAsync(CreateCommentReqDto request)
        {
            var userId = UserId;
            if (userId == Guid.Empty)
            {
                throw new AuthException(ErrorCode.InvalidCredentials, "Vui lòng đăng nhập để thực hiện thao tác này.");
            }

            var comment = new CommentEntity
            {
                ProductId = request.ProductId,
                UserId = userId,
                Content = request.Content,
                Rating = Math.Clamp(request.Rating, 1, 5),
                ImageUrls = request.ImageUrls != null ? string.Join(";", request.ImageUrls) : null,
                CreatedBy = userId.ToString()
            };

            await _commentManager.CreateAsync(comment);

            return new ApiResponse<CommentResDto>
            {
                Data = new CommentResDto
                {
                    Id = comment.Id,
                    ProductId = comment.ProductId,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    ImageUrls = request.ImageUrls,
                    CreatedAt = comment.CreatedAt
                }
            };
        }

        public async Task<ApiResponse<List<CommentResDto>>> GetProductCommentsAsync(Guid productId)
        {
            var comments = await _commentManager.GetProductCommentsAsync(productId);

            var result = comments.Select(c => new CommentResDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                UserId = c.UserId,
                UserName = c.User?.FullName ?? "Người dùng",
                UserAvatar = c.User?.AvatarUrl,
                Content = c.Content,
                Rating = c.Rating,
                ImageUrls = !string.IsNullOrEmpty(c.ImageUrls) ? c.ImageUrls.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() : null,
                CreatedAt = c.CreatedAt
            }).ToList();

            return new ApiResponse<List<CommentResDto>>
            {
                Data = result
            };
        }
    }
}
