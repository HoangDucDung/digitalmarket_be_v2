using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IProductService _productService => _lazyloadProvider.LazyGetRequiredService<IProductService>();

        [HttpGet("tree")]
        [ProducesResponseType(typeof(CategoryTreeResDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryTreeResDto>> GetCategoryTree([FromQuery] CategoryTreeReqDto req)
        {
            var result = await _productService.GetCategoryTreeAsync(req);
            return Ok(result);
        }
    }
}
