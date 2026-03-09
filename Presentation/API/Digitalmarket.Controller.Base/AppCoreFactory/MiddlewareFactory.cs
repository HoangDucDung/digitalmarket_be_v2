using Project.DigitalMarket.Host.Base.Middleware;

namespace Digitalmarket.Controller.Base.AppCoreFactory
{
    public static class MiddlewareFactory
    {
        public static IApplicationBuilder MiddlewareRegistration(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<AuthenMiddleware>();
            builder.UseMiddleware<ApplicationMiddleware>();

            return builder;
        }
    }
}
