using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Stocks.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly ILogger<InstrumentsController> logger;
        private readonly StocksContext stocksContext = new StocksContext();
        //private readonly Setting[] settings;

        public InstrumentsController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<InstrumentsController>();
        }

        [HttpGet]
        public async void Update()
        {
            logger.LogInformation($"Update started {DateTime.Now}");

            using HttpClient httpClient = new HttpClient();
            Authorization authorization = await Modules.TdAmeritrade.Authorization.Update();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Plain));

            for (ulong i = 0; i < 1000000000; i++)
            {
                using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://api.tdameritrade.com/v1/instruments/" + i);
                string data = httpResponseMessage.Content.ReadAsStringAsync().Result;
                using Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
                using StreamReader streamReader = new StreamReader(stream);
            }

            logger.LogInformation($"Update completed {DateTime.Now}");
        }
    }
}
