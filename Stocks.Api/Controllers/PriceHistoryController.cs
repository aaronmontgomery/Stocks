using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stocks.Entities;
using Stocks.Models;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PriceHistoryController : Controller
    {
        private readonly ILogger<PriceHistoryController> _logger;
        private StocksContext _stocksContext;
        private readonly Dictionary<string, string> _settings;
        private string _priceHistoryUri;

        public PriceHistoryController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PriceHistoryController>();
            _stocksContext = new StocksContext();
            _settings = _stocksContext.Setting.ToDictionary(x => x.Key, x => x.Value);
        }

        [HttpGet]
        public async void Update()
        {
            _logger.LogInformation($"Update started {DateTime.Now}");
            Stopwatch stopwatch = new Stopwatch();
            Ticker[] tickers = _stocksContext.Ticker.ToArray();
            for (ushort i = 0; i < tickers.Count(); i++)
            {
                try
                {
                    stopwatch.Stop();
                    long t = 501 - stopwatch.ElapsedMilliseconds;
                    if (t > 0)
                    {
                        Thread.Sleep((int)t);
                    }

                    stopwatch.Restart();
                    _stocksContext = new StocksContext();
                    long start = _stocksContext.PriceHistory.Count(x => x.Symbol == tickers[i].Symbol) > 0 ? new DateTimeOffset(_stocksContext.PriceHistory.Where(x => x.Symbol == tickers[i].Symbol).OrderBy(x => x.DateTime).Last().DateTime.Date).AddDays(1).ToUnixTimeMilliseconds() : new DateTimeOffset(new DateTime(2015, 1, 1)).ToUnixTimeMilliseconds();
                    long end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    _priceHistoryUri = $"{_settings["MarketDataUri"]}/{tickers[i].Symbol}/pricehistory?apikey={_settings["ApiKey"]}&periodType=year&frequencyType=daily&startDate={start}&endDate={end}";
                    WebRequest webRequest = WebRequest.CreateHttp(_priceHistoryUri);
                    using WebResponse webResponse = webRequest.GetResponse();
                    using Stream stream = webResponse.GetResponseStream();
                    using StreamReader streamReader = new StreamReader(stream);
                    string json = streamReader.ReadToEnd();
                    json = Toolbox.Json.RemoveEntry(json, "NaN");
                    StockCandle stockCandles = Newtonsoft.Json.JsonConvert.DeserializeObject<StockCandle>(json);
                    foreach (Candle candle in stockCandles.Candles)
                    {
                        PriceHistory priceHistory = new PriceHistory()
                        {
                            Symbol = tickers[i].Symbol,
                            Open = candle.Open,
                            High = candle.High,
                            Low = candle.Low,
                            Close = candle.Close,
                            Volume = candle.Volume,
                            DateTime = DateTime.UnixEpoch.AddMilliseconds(candle.DateTime),
                            Updated = DateTime.UtcNow
                        };
                        _stocksContext.PriceHistory.Add(priceHistory);
                        await _stocksContext.SaveChangesAsync();
                    }

                    _logger.LogInformation($"Update successful {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                }

                catch (WebException webException)
                {
                    using HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
                    if (httpWebResponse is HttpWebResponse)
                    {
                        switch (httpWebResponse.StatusCode)
                        {
                            case HttpStatusCode.TooManyRequests:
                                _logger.LogWarning($"Update too many requests {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                                i--;
                                break;
                            case HttpStatusCode.NotFound:
                                _logger.LogWarning($"Update not found {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                                break;
                            case HttpStatusCode.BadRequest:
                                _logger.LogError($"Update bad request {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                                break;
                            case HttpStatusCode.BadGateway:
                                _logger.LogError($"Update bad gateway {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                                break;
                            default:
                                _logger.LogWarning($"Update *default* {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                                break;
                        }
                    }

                    else
                    {
                        _logger.LogWarning($"Update timeout {tickers[i].Symbol} {i + 1}/{tickers.Count()}");
                        i--;
                    }
                }
            }

            _logger.LogInformation($"Update completed {DateTime.Now}");
        }
    }
}
