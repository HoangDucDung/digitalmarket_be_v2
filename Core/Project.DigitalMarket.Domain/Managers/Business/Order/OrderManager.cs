using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Managers.Business.Order
{
    public class OrderManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IOrderManager
    {
        private IOrderRepository _orderRepository => _lazyloadProvider.LazyGetRequiredService<IOrderRepository>();
        private ICartRepository _cartRepository => _lazyloadProvider.LazyGetRequiredService<ICartRepository>();
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        public async Task<OrderEntity> CheckoutCartAsync(Guid userId, string paymentMethod, string? note)
        {
            var cartItems = await _cartRepository.GetByCondition(x => x.UserId == userId && x.IsSelected)
                .Include(x => x.Product)
                .ThenInclude(p => p.Variants)
                .ToListAsync();

            if (!cartItems.Any()) throw new BusinessException(ErrorCode.EmptyCart, "Giỏ hàng của bạn đang trống.");

            if (cartItems.Any(x => x.Product.SellerId == userId))
                throw new BusinessException(ErrorCode.CannotBuyOwnProduct, "Giỏ hàng của bạn chứa sản phẩm do chính bạn đăng bán.");

            // Tính toán giá trị
            decimal subtotal = cartItems.Sum(x => GetUnitPrice(x.Product) * x.Quantity);
            decimal discount = 0; // Tạm thời
            decimal total = subtotal - discount;

            var order = new OrderEntity
            {
                BuyerId = userId,
                OrderCode = "ORD" + DateTime.Now.Ticks.ToString().Substring(10),
                Subtotal = subtotal,
                DiscountTotal = discount,
                TotalAmount = total,
                PaymentMethod = paymentMethod,
                BuyerNote = note,
                Status = OrderConstants.Status.Pending
            };

            foreach (var item in cartItems)
            {
                order.Items.Add(new OrderItemEntity
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    OriginalPrice = GetOriginalPrice(item.Product),
                    UnitPrice = GetUnitPrice(item.Product),
                    Subtotal = GetUnitPrice(item.Product) * item.Quantity
                });

                // Xóa khỏi giỏ hàng
                _cartRepository.Delete(item);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task<OrderEntity> DirectPurchaseAsync(Guid userId, Guid productId, int quantity, string paymentMethod, string? note)
        {
            var product = await _productRepository.GetByCondition(x => x.Id == productId && x.IsActive && !x.IsDeleted)
                .Include(x => x.Variants)
                .FirstOrDefaultAsync();
            if (product == null) throw new BusinessException(ErrorCode.ProductNotAvailable, "Sản phẩm không khả dụng.");

            if (product.SellerId == userId)
                throw new BusinessException(ErrorCode.CannotBuyOwnProduct, "Bạn không thể mua sản phẩm do chính mình đăng bán.");

            decimal unitPrice = GetUnitPrice(product);
            decimal subtotal = unitPrice * quantity;

            var order = new OrderEntity
            {
                BuyerId = userId,
                OrderCode = "ORD" + DateTime.Now.Ticks.ToString().Substring(10),
                Subtotal = subtotal,
                TotalAmount = subtotal,
                PaymentMethod = paymentMethod,
                BuyerNote = note,
                Status = OrderConstants.Status.Pending,
                Items = new List<OrderItemEntity>
                {
                    new OrderItemEntity
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        Quantity = quantity,
                        OriginalPrice = GetOriginalPrice(product),
                        UnitPrice = unitPrice,
                        Subtotal = subtotal
                    }
                }
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order;
        }

        private static decimal GetUnitPrice(ProductEntity product)
        {
            return product.Variants.Where(v => v.IsActive).OrderBy(v => v.Price).Select(v => v.Price).FirstOrDefault();
        }

        private static decimal GetOriginalPrice(ProductEntity product)
        {
            return product.Variants.Where(v => v.IsActive).OrderBy(v => v.Price).Select(v => v.OriginalPrice ?? v.Price).FirstOrDefault();
        }

        public async Task<List<OrderEntity>> GetUserOrdersAsync(Guid userId)
        {
            return await _orderRepository.GetByCondition(x => x.BuyerId == userId)
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderEntity> GetOrderDetailAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetByCondition(x => x.Id == orderId && x.BuyerId == userId)
                .Include(x => x.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();
            if (order == null) throw new BusinessException(ErrorCode.OrderNotFound, "Đơn hàng không tồn tại.");
            return order;
        }

        public async Task CancelOrderAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetByCondition(x => x.Id == orderId && x.BuyerId == userId).FirstOrDefaultAsync();
            if (order == null) throw new BusinessException(ErrorCode.OrderNotFound, "Đơn hàng không tồn tại.");
            if (order.Status != OrderConstants.Status.Pending) throw new BusinessException(ErrorCode.OnlyPendingOrderAllowed, "Chỉ có thể hủy đơn hàng đang chờ.");

            order.Status = OrderConstants.Status.Cancelled;
            order.UpdatedAt = GenerateExtentions.Now;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
