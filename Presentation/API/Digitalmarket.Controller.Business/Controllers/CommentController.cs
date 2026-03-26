using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class CommentController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private ICommentService _commentService => _lazyloadProvider.LazyGetRequiredService<ICommentService>();

        /// <summary>
        /// Tạo mới bình luận cho sản phẩm.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CommentResDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentReqDto request)
        {
            var result = await _commentService.CreateCommentAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách bình luận của sản phẩm.
        /// </summary>
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<CommentResDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductComments(Guid productId)
        {
            var result = await _commentService.GetProductCommentsAsync(productId);
            return Ok(result);
        }
    }
}
