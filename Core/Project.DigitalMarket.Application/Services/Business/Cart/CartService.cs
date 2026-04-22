using Project.DigitalMarket.Application.Contract.DTOs.Business.Cart;
using Project.DigitalMarket.Application.Contract.Services.Business.Cart;
using Project.DigitalMarket.Domain.Managers.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;

namespace Project.DigitalMarket.Application.Services.Business.Cart
{
    internal sealed class CartService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<CartService>(lazyloadProvider), ICartService
    {
        private ICartManager _cartManager => _lazyloadProvider.LazyGetRequiredService<ICartManager>();
        private ICartRepository _cartRepository => _lazyloadProvider.LazyGetRequiredService<ICartRepository>();

        public async Task AddToCartAsync(Guid productId, int quantity)
        {
            await _cartManager.AddToCartAsync(UserId, productId, quantity);
        }

        public async Task UpdateQuantityAsync(Guid cartItemId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(UserId, cartItemId);
            if (cartItem == null) throw new BusinessException(ErrorCode.CartItemNotFound, "Mục giỏ hàng không tồn tại.");

            cartItem.Quantity = quantity;
            _cartRepository.Update(cartItem);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(Guid cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(UserId, cartItemId);
            if (cartItem != null)
            {
                _cartRepository.Delete(cartItem);
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task<List<CartItemResultDto>> GetMyCartAsync()
        {
            var cartItems = await _cartRepository.GetCartByUserIdAsync(UserId);
            return _mapper.Map<List<CartItemResultDto>>(cartItems);
        }

        public async Task ClearCartAsync()
        {
            await _cartRepository.ClearCartAsync(UserId);
        }
    }
}
