using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Modules.TdAmeritrade;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PriceHistoryController : ControllerBase
    {
        private readonly ILogger<PriceHistoryController> _logger;
        private readonly IPriceHistory _priceHistory;
        
        public PriceHistoryController(ILoggerFactory loggerFactory, IPriceHistory priceHistory)
        {
            _logger = loggerFactory.CreateLogger<PriceHistoryController>();
            _priceHistory = priceHistory;
        }

        [HttpGet]
        public async Task<IActionResult> GetPriceHistoryAsync(CancellationToken cancellationToken)
        {
            IActionResult actionResult;

            try
            {
                _logger.LogInformation("GetPriceHistoryAsync {now}", DateTime.Now);
                await _priceHistory.GetPriceHistoryAsync(cancellationToken);
                actionResult = Ok();
            }
            
            catch
            {
                actionResult = BadRequest();
            }
            
            return actionResult;
        }
    }
}
