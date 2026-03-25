using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Cart
{
    public class CartRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<CartItemEntity>(lazyloadProvider), ICartRepository
    {
    }
}
