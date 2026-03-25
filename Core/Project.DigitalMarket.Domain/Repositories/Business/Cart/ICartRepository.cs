using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Cart
{
    public interface ICartRepository : IRepositoryBase<CartItemEntity>
    {
    }
}
