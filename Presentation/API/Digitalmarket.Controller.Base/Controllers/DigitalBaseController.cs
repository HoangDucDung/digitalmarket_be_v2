using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Base.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DigitalBaseController : ControllerBase
    {
        protected ILazyloadProvider _lazyloadProvider;
        public DigitalBaseController(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }
    }
}
