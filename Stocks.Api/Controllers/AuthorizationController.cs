using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AuthorizationController>();
        }

        [HttpGet]
        public void PostAccessToken(string code)
        {
            _logger.LogInformation($"PostAccessToken {DateTime.Now}");
            Modules.TdAmeritrade.Authorization.Update(code);
        }
    }
}
