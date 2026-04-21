using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Application.Contract.Services.Auths;
using Project.DigitalMarket.Application.Contract.Services.Business.Cart;
using Project.DigitalMarket.Application.Contract.Services.Business.Order;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Seller;
using Project.DigitalMarket.Application.Contract.Services.Business.Wallet;
using Project.DigitalMarket.Application.Contract.Services.Mails;
using Project.DigitalMarket.Application.Services.Auths;
using Project.DigitalMarket.Application.Services.Business.Cart;
using Project.DigitalMarket.Application.Services.Business.Order;
using Project.DigitalMarket.Application.Services.Business.Product;
using Project.DigitalMarket.Application.Services.Business.Seller;
using Project.DigitalMarket.Application.Services.Business.Wallet;
using Project.DigitalMarket.Application.Services.Mails;

namespace Project.DigitalMarket.Application
{
    public static class ServiceFactory
    {
        public static IServiceCollection AddAuthServiceFactory(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }

        public static IServiceCollection AddBusinessServiceFactory(this IServiceCollection services)
        {
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IWalletService, WalletService>();
            return services;
        }
    }
}
