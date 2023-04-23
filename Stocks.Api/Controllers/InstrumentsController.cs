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
    public class InstrumentsController : ControllerBase
    {
        private readonly ILogger<InstrumentsController> _logger;
        private readonly IInstruments _instruments;
        
        public InstrumentsController(ILoggerFactory loggerFactory, IInstruments instruments)
        {
            _logger = loggerFactory.CreateLogger<InstrumentsController>();
            _instruments = instruments;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstrumentsAsync(CancellationToken cancellationToken)
        {
            IActionResult actionResult;
            
            try
            {
                _logger.LogInformation("GetInstrumentsAsync {now}", DateTime.Now);
                await _instruments.GetInstrumentsAsync(cancellationToken);
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
