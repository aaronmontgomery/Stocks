using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Entities;

namespace Stocks.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly ILogger<InstrumentsController> logger;
        private readonly StocksContext stocksContext = new StocksContext();
        private readonly Dictionary<string, string> settings;

        public InstrumentsController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<InstrumentsController>();
            settings = stocksContext.Setting.ToDictionary(x => x.Key, x => x.Value);
        }

        [HttpGet]
        public async Task GetInstrument()
        {
            logger.LogInformation($"GetInstrument started {DateTime.Now}");

            using HttpClient httpClient = new HttpClient();
            Authorization authorization = await Modules.TdAmeritrade.Authorization.Update();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);

            uint end = 1000000000;
            for (uint i = 0; i < end; i++)
            {
                string value = i.ToString().PadLeft(end.ToString().Length - i.ToString().Length).Replace(' ', '0');
                using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"https://api.tdameritrade.com/v1/instruments/{value}?apikey={settings["ApiKey"]}"); // 594918104
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                using Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                using StreamReader streamReader = new StreamReader(stream);
            }

            logger.LogInformation($"GetInstrument completed {DateTime.Now}");
        }
    }
}
