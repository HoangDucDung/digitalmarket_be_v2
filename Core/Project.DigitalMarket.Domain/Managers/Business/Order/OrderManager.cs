using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.Extensions.Extensions;
using Project.DigitalMarket.Domain.Managers.Business.Wallet;

namespace Project.DigitalMarket.Domain.Managers.Business.Order
{
    internal sealed class OrderManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IOrderManager
    {
        private IOrderRepository _orderRepository => _lazyloadProvider.LazyGetRequiredService<IOrderRepository>();
        private ICartRepository _cartRepository => _lazyloadProvider.LazyGetRequiredService<ICartRepository>();
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();
        private IWalletManager _walletManager => _lazyloadProvider.LazyGetRequiredService<IWalletManager>();

        public async Task<OrderEntity> CheckoutCartAsync(Guid userId, string paymentMethod, string? note)
        {
            var cartItems = await _cartRepository.GetSelectedItemsWithProductByUserIdAsync(userId);

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
                // Cập nhật tồn kho
                await UpdateProductStockAsync(item.Product, item.Quantity);

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

            // Xử lý thanh toán nếu dùng ví
            if (paymentMethod == OrderConstants.PaymentMethod.InternalBalance)
            {
                await _walletManager.ProcessTransactionAsync(userId, -total, 
                    WalletConstants.TransactionType.Payment, 
                    $"Thanh toán đơn hàng {order.OrderCode}", 
                    order.OrderCode);
                order.Status = OrderConstants.Status.Processing;
            }

            await _orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task<OrderEntity> DirectPurchaseAsync(Guid userId, Guid productId, int quantity, string paymentMethod, string? note)
        {
            var product = await _productRepository.GetActiveWithVariantsByIdAsync(productId);
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

            // Cập nhật tồn kho
            await UpdateProductStockAsync(product, quantity);

            await _orderRepository.AddAsync(order);

            // Xử lý thanh toán nếu dùng ví
            if (paymentMethod == OrderConstants.PaymentMethod.InternalBalance)
            {
                await _walletManager.ProcessTransactionAsync(userId, -subtotal, 
                    WalletConstants.TransactionType.Payment, 
                    $"Thanh toán đơn hàng {order.OrderCode}", 
                    order.OrderCode);
                order.Status = OrderConstants.Status.Processing;
            }

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

        private async Task UpdateProductStockAsync(ProductEntity product, int quantity)
        {
            var variant = product.Variants
                .Where(v => v.IsActive)
                .OrderBy(v => v.Price)
                .FirstOrDefault();

            if (variant == null)
                throw new BusinessException(ErrorCode.ProductNotAvailable, $"Sản phẩm '{product.Name}' không có biến thể khả dụng.");

            if (variant.StockQuantity < quantity)
                throw new BusinessException(ErrorCode.OutOfStock, $"Sản phẩm '{product.Name}' - {variant.VariantName} không đủ tồn kho (Còn lại: {variant.StockQuantity}).");

            variant.StockQuantity -= quantity;
            variant.InventoryMovements.Add(new ProductInventoryMovementEntity
            {
                ChangeType = "Order",
                QuantityDelta = -quantity,
                Note = "Trừ tồn kho cho đơn hàng mới",
                CreatedAt = GenerateExtentions.Now,
                CreatedBy = "System"
            });

            _productRepository.Update(product);
        }
    }
}
