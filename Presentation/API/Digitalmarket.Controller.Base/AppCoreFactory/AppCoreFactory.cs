using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Host.Base.Middleware;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Base.AppCoreFactory
{
    /// <summary>
    /// Class DI factory để tạo các instance của các lớp trong AppCore, giúp tách rời việc khởi tạo và quản lý các đối tượng, đồng thời hỗ trợ việc mở rộng và bảo trì mã nguồn dễ dàng hơn.
    /// </summary>
    public static class AppCoreFactory
    {
        /// <summary>
        /// Đăng ký service cho các lớp trong AppCore, bao gồm các provider hỗ trợ lazy loading và caching, 
        /// giúp tối ưu hiệu năng và quản lý tài nguyên hiệu quả hơn.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddLazyloadFactory(this IServiceCollection service)
        {
            service.AddHttpContextAccessor();
            service.AddScoped<ICachedServiceProviderBase, CachedServiceProviderBase>();
            service.AddScoped<ILazyloadProvider, LazyloadProvider>();
            return service;
        }

        /// <summary>
        /// Cấu hình middeware cho ứng dụng.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder MiddlewareRegistration(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ApplicationMiddleware>();

            return builder;
        }
    }
}
