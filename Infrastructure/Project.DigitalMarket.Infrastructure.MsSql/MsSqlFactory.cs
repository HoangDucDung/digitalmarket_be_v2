using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Domain.Repositories.Auths;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Domain.Repositories.Business.Seller;
using Project.DigitalMarket.Domain.Repositories.Business.Wallet;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Auths;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Cart;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Order;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Product;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Seller;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Wallet;

namespace Project.DigitalMarket.Infrastructure.MsSql
{
    public static class MsSqlFactory
    {
        public static IServiceCollection AddAuthMsSqlFactory(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
        }

        public static IServiceCollection AddBusinessMsSqlFactory(this IServiceCollection services)
        {
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<IKycRepository, KycRepository>();
            services.AddScoped<IFinancialRepository, FinancialRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
