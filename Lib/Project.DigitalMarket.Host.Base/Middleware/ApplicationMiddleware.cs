using Microsoft.AspNetCore.Http;
using Project.DigitalMarket.Libs.Exceptions;

namespace Project.DigitalMarket.Host.Base.Middleware
{
    public class ApplicationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApplicationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseHttpStatusCodeException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
