using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stocks.Entities;

namespace Stocks.Modules.TdAmeritrade
{
    public class Instruments : IInstruments
    {
        private readonly ILogger _logger;
        private readonly IDataAdapter _dataAdapter;
        
        public Instruments(IDataAdapter dataAdapter, ILogger<Instruments> logger)
        {
            _logger = logger;
            _dataAdapter = dataAdapter;
        }
        
        public async Task GetInstrumentsAsync(CancellationToken cancellationToken)
        {
            DateTime dateTime;
            Instrument ins;
            IAsyncEnumerable<Instrument> instruments;
            
            _logger.LogInformation("GetInstrumentsAsync started {now}", DateTime.Now);

            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            
            for (byte i = Convert.ToByte('A'); i <= Convert.ToByte('Z'); i++)
            {
                try
                {
                    using (var stocksDbContext = await _dataAdapter.StocksDbContextFactory.CreateDbContextAsync(cancellationToken))
                    {
                        _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["InstrumentsUrl"]}?apikey={_dataAdapter.Settings["ApiKey"]}&symbol={Convert.ToChar(i)}.*&projection=symbol-regex", null, cancellationToken);
                        _dataAdapter.Json = Toolbox.Json.RemoveTopLevelKeys(_dataAdapter.Json);
                        instruments = JsonSerializer.Deserialize<IAsyncEnumerable<Instrument>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
                        await foreach (Instrument instrument in instruments)
                        {
                            try
                            {
                                _logger.LogInformation("GetInstrumentsAsync processing {exchange} {symbol}", instrument.Exchange, instrument.Symbol);

                                dateTime = DateTime.UtcNow;
                                ins = await stocksDbContext.FindAsync<Instrument>(new object[] { instrument.Exchange, instrument.Symbol }, cancellationToken);
                                if (ins == null)
                                {
                                    instrument.Created = dateTime;
                                    instrument.Updated = dateTime;
                                    await stocksDbContext.AddAsync(instrument, cancellationToken);
                                }

                                else
                                {
                                    ins.Cusip = instrument.Cusip;
                                    ins.AssetType = instrument.AssetType;
                                    ins.Updated = dateTime;
                                    stocksDbContext.Update(ins);
                                }
                            }

                            catch
                            {
                                _logger.LogError("GetInstrumentsAsync error updaing {symbol}", instrument.Symbol);
                            }

                            finally
                            {
                                _logger.LogInformation("GetInstrumentsAsync processed {exchange} {symbol}", instrument.Exchange, instrument.Symbol);
                            }
                        }

                        await stocksDbContext.SaveChangesAsync(cancellationToken);
                    }
                }

                catch (Exception exception)
                {
                    _logger.LogError("GetInstrumentsAsync {message}", exception.Message);
                }
            }
            
            _logger.LogInformation("GetInstrumentsAsync completed {now}", DateTime.Now);
        }
    }
}
