using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IOrderService _orderService => _lazyloadProvider.LazyGetRequiredService<IOrderService>();

    }
}
