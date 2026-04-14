using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.DigitalMarket.Libs.DependencyInjection;
using System.Security.Claims;

namespace Project.DigitalMarket.Application
{
    /// <summary>
    /// Lớp Base cho tất cả các Service trong Application layer
    /// </summary>
    public abstract class DigitalMarketServiceBase<T>(ILazyloadProvider lazyloadProvider)
    {
        protected readonly ILazyloadProvider _lazyloadProvider = lazyloadProvider;
        protected IMapper _mapper => _lazyloadProvider.LazyGetRequiredService<IMapper>();
        protected IHttpContextAccessor _httpContextAccessor => _lazyloadProvider.LazyGetRequiredService<IHttpContextAccessor>();
        protected ILogger _logger => _lazyloadProvider.LazyGetRequiredService<ILoggerFactory>().CreateLogger(typeof(T));

        protected Guid UserId
        {
            get
            {
                var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(userIdStr, out var userId) ? userId : Guid.Empty;
            }
        }

        protected string UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }
}
