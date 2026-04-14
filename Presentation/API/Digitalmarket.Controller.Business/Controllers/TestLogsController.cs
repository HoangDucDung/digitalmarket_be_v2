using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    public class TestLogsController(ILazyloadProvider lazyloadProvider) : DigitalBaseController<TestLogsController>(lazyloadProvider)
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("This is an information log from TestLogsController.");
            _logger.LogWarning("This is a warning log from TestLogsController.");
            _logger.LogError("This is an error log from TestLogsController.");
            Console.WriteLine("This is a log from TestLogsController.");
            return Ok("Logs have been written. Check your logging output.");
        }
    }
}
