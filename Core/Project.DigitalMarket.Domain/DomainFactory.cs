using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Domain.Managers.Auths;
using Project.DigitalMarket.Domain.Managers.Business.Cart;
using Project.DigitalMarket.Domain.Managers.Business.Order;
using Project.DigitalMarket.Domain.Managers.Business.Product;
using Project.DigitalMarket.Domain.Managers.Business.Seller;
using Project.DigitalMarket.Domain.Managers.Business.Wallet;

namespace Project.DigitalMarket.Domain
{
    public static class DomainFactory
    {

        public static IServiceCollection AddAuthDomainFactory(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            return services;
        }

        public static IServiceCollection AddBusinessDomainFactory(this IServiceCollection services)
        {
            services.AddScoped<IWalletManager, WalletManager>();
            services.AddScoped<ISellerManager, SellerManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            return services;
        }
    }
}
