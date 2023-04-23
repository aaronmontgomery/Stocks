using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stocks.Entities;
using Stocks.Models;

namespace Stocks.Modules.TdAmeritrade
{
    public class PriceHistory : IPriceHistory
    {
        private readonly ILogger<PriceHistory> _logger;
        private readonly IDataAdapter _dataAdapter;
        
        public PriceHistory(ILoggerFactory loggerFactory, IDataAdapter dataAdapter)
        {
            _logger = loggerFactory.CreateLogger<PriceHistory>();
            _dataAdapter = dataAdapter;
        }

        public async Task GetPriceHistoryAsync(CancellationToken cancellationToken)
        {
            long start;
            long end;
            int count;
            StockCandles stockCandle;
            IAsyncEnumerable<Models.TdAmeritrade.Account.Instrument> instruments;

            _logger.LogInformation("GetPriceHistoryAsync started {now}", DateTime.Now);

            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };

            using (var stocksDbContext = await _dataAdapter.StocksDbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                instruments = stocksDbContext.Instrument.AsAsyncEnumerable();
                await foreach (var instrument in instruments)
                {
                    try
                    {
                        using (var context = await _dataAdapter.StocksDbContextFactory.CreateDbContextAsync(cancellationToken))
                        {
                            count = context.PriceHistory.Count(x => x.Exchange == instrument.Exchange && x.Symbol == instrument.Symbol);
                            start = count > 0 ? new DateTimeOffset(context.PriceHistory.Where(x => x.Exchange == instrument.Exchange && x.Symbol == instrument.Symbol).OrderBy(x => x.DateTime).Last().DateTime.Date).AddDays(1).ToUnixTimeMilliseconds() : new DateTimeOffset(Convert.ToDateTime(_dataAdapter.Settings["BeginDateTime"])).ToUnixTimeMilliseconds();
                            end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["MarketDataUrl"]}/{instrument.Symbol}/pricehistory?apikey={_dataAdapter.Settings["ApiKey"]}&periodType=year&frequencyType=daily&startDate={start}&endDate={end}", null, cancellationToken);
                            _dataAdapter.Json = Toolbox.Json.RemoveEntry(_dataAdapter.Json, "NaN");
                            stockCandle = JsonSerializer.Deserialize<StockCandles>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
                            if (stockCandle.Empty)
                            {
                                _logger.LogInformation("GetPriceHistoryAsync empty {exchange} {symbol}", instrument.Exchange, instrument.Symbol);
                            }

                            else
                            {
                                await context.AddRangeAsync(stockCandle.Candles.Select(x => new Entities.PriceHistory()
                                {
                                    Symbol = instrument.Symbol,
                                    DateTime = DateTime.UnixEpoch.AddMilliseconds(x.DateTime),
                                    Exchange = instrument.Exchange,
                                    Open = x.Open,
                                    Close = x.Close,
                                    High = x.High,
                                    Low = x.Low,
                                    Volume = x.Volume,
                                    //Instrument = instrument,
                                    Updated = DateTime.UtcNow
                                }).OrderBy(x => x.DateTime), cancellationToken);
                                await context.SaveChangesAsync(cancellationToken);

                                _logger.LogInformation("GetPriceHistoryAsync added {exchange} {symbol}", instrument.Exchange, instrument.Symbol);
                            }
                        }
                    }

                    catch (Exception exception)
                    {
                        _logger.LogWarning("GetPriceHistoryAsync {exchange} {symbol} {message}", instrument.Exchange, instrument.Symbol, exception.Message);
                    }
                }
            }

            _logger.LogInformation("GetPriceHistoryAsync completed {now}", DateTime.Now);
        }
    }
}
