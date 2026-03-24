using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Services.Business
{
    public class OrderService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IOrderService
    {
    
    }
}
