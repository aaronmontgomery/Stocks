using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Entities;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IDataAdapter _dataAdapter;
        
        public AuthorizationController(ILoggerFactory loggerFactory, IDataAdapter dataAdapter)
        {
            _logger = loggerFactory.CreateLogger<AuthorizationController>();
            _cancellationTokenSource = new();
            _dataAdapter = dataAdapter;
        }

        [HttpGet]
        public async Task<IActionResult> PostAccessTokenAsync(string code, CancellationToken cancellationToken)
        {
            IActionResult actionResult;

            try
            {
                _logger.LogInformation("PostAccessTokenAsync {now}", DateTime.Now);
                actionResult = Ok(await _dataAdapter.UpdateTokenAsync(code, cancellationToken));
            }

            catch
            {
                actionResult = BadRequest();
            }
            
            return actionResult;
        }
    }
}
