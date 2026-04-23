using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Domain.Models.Commons;
using Microsoft.Extensions.Logging;
using Project.DigitalMarket.Domain.Share.Constants.Auths;

namespace Digitalmarket.Controller.Base.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DigitalBaseController<T>(ILazyloadProvider lazyloadProvider) : ControllerBase
    {
        protected ILazyloadProvider _lazyloadProvider = lazyloadProvider;
        protected ILogger _logger => _lazyloadProvider.LazyGetRequiredService<ILoggerFactory>().CreateLogger(typeof(T));

        private UserContext? _userContext;

        /// <summary>
        /// Thông tin người dùng hiện tại trích xuất từ JWT token
        /// </summary>
        protected UserContext UserContext
        {
            get
            {
                if (_userContext == null)
                {
                    _userContext = new UserContext
                    {
                        UserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : Guid.Empty,
                        Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                        FullName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                        Role = User.FindFirstValue(AppClaimTypes.Role) ?? string.Empty
                    };
                }
                return _userContext;
            }
        }
    }
}
