using Project.DigitalMarket.Application.Contract.DTOs.Business.Cart;
using Project.DigitalMarket.Application.Contract.Services.Business.Cart;
using Project.DigitalMarket.Domain.Managers.Business.Cart;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business.Cart
{
    public class CartService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), ICartService
    {
        private ICartManager _cartManager => _lazyloadProvider.LazyGetRequiredService<ICartManager>();

        public async Task AddToCartAsync(Guid productId, int quantity)
        {
            await _cartManager.AddToCartAsync(UserId, productId, quantity);
        }

        public async Task UpdateQuantityAsync(Guid cartItemId, int quantity)
        {
            await _cartManager.UpdateQuantityAsync(UserId, cartItemId, quantity);
        }

        public async Task RemoveFromCartAsync(Guid cartItemId)
        {
            await _cartManager.RemoveFromCartAsync(UserId, cartItemId);
        }

        public async Task<List<CartItemResultDto>> GetMyCartAsync()
        {
            var cartItems = await _cartManager.GetUserCartAsync(UserId);
            return _mapper.Map<List<CartItemResultDto>>(cartItems);
        }

        public async Task ClearCartAsync()
        {
            await _cartManager.ClearCartAsync(UserId);
        }
    }
}
