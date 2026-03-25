using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Order
{
    public class OrderRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<OrderEntity>(lazyloadProvider), IOrderRepository
    {
    }
}
