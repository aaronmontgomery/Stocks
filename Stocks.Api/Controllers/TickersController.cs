using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Entities;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TickersController : Controller
    {
        private readonly ILogger<TickersController> _logger;
        private StocksContext _stocksContext;
        private readonly Setting[] _settings;

        public TickersController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TickersController>();
            _stocksContext = new StocksContext();
            _settings = _stocksContext.Setting.Where(x => x.Type == "Exchange").ToArray();
        }

        [HttpGet]
        public async void Update()
        {
            _logger.LogInformation($"Update started {DateTime.Now}");
            foreach (Setting setting in _settings)
            {
                /*
                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Plain));
                using HttpResponseMessage httpResponseMessage = httpClient.GetAsync(setting.Value).Result;
                string data = httpResponseMessage.Content.ReadAsStringAsync().Result;
                using Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
                using StreamReader streamReader = new StreamReader(stream);
                */
                WebRequest webRequest = WebRequest.CreateHttp(setting.Value);
                webRequest.Headers.Add(HttpRequestHeader.Accept, MediaTypeNames.Text.Plain);
                using WebResponse webResponse = webRequest.GetResponse();
                using Stream stream = webResponse.GetResponseStream();
                using StreamReader streamReader = new StreamReader(stream);
                IEnumerable<string> header = streamReader.ReadLine().Split("\",", StringSplitOptions.RemoveEmptyEntries).Select(x => WebUtility.HtmlDecode(x.Trim(new char[] { '\"', ' ' })));
                while (!streamReader.EndOfStream)
                {
                    _stocksContext = new StocksContext();
                    Dictionary<string, string> data = header.Zip(streamReader.ReadLine().Split("\",", StringSplitOptions.RemoveEmptyEntries).Select(x => WebUtility.HtmlDecode(x.Trim(new char[] { '\"', ' ' })))).ToDictionary(x => x.First, x => x.Second);
                    if (_stocksContext.Ticker.Any(x => x.Symbol == data["Symbol"]))
                    {
                        Ticker ticker = _stocksContext.Ticker.Single(x => x.Symbol == data["Symbol"]);
                        ticker.Symbol = data["Symbol"];
                        ticker.Name = data["Name"];
                        ticker.LastSale = data["LastSale"];
                        ticker.MarketCap = data["MarketCap"];
                        ticker.IpoYear = data["IPOyear"];
                        ticker.Sector = data["Sector"];
                        ticker.Industry = data["industry"];
                        ticker.SummaryQuote = data["Summary Quote"];
                        ticker.Updated = DateTime.UtcNow;
                        _stocksContext.Ticker.Update(ticker);
                        _logger.LogInformation($"Update updated {ticker.Symbol}");
                    }

                    else
                    {
                        Ticker ticker = new Ticker()
                        {
                            Symbol = data["Symbol"],
                            Name = data["Name"],
                            LastSale = data["LastSale"],
                            MarketCap = data["MarketCap"],
                            IpoYear = data["IPOyear"],
                            Sector = data["Sector"],
                            Industry = data["industry"],
                            SummaryQuote = data["Summary Quote"],
                            Updated = DateTime.UtcNow
                        };
                        _stocksContext.Ticker.Add(ticker);
                        _logger.LogInformation($"Update added {ticker.Symbol}");
                    }

                    await _stocksContext.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Update completed {DateTime.Now}");
        }
    }
}
