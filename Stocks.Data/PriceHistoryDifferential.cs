using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Stocks.Models;

namespace Stocks.Data
{
    public static partial class CompiledQueries
    {
        public static IEnumerable<PriceHistoryDifferential> PriceHistoryDifferential(DateTimeRange dateTimeRange)
        {
            Entities.StocksContext stocksContext = new Entities.StocksContext();
            List<PriceHistoryDifferential> priceHistoryDifferentials = new List<PriceHistoryDifferential>();
            Dictionary<string, Dictionary<Entities.PriceHistory, decimal>> dailyAverages = new Dictionary<string, Dictionary<Entities.PriceHistory, decimal>>();
            IEnumerable<IGrouping<string, Entities.PriceHistory>> priceHistoryGroupings = stocksContext.PriceHistory.Where(x => dateTimeRange.Start.Date <= x.DateTime.Date && x.DateTime.Date <= dateTimeRange.End.Date).OrderBy(x => x.DateTime).ThenBy(x => x.Symbol).AsNoTracking().ToArray().GroupBy(x => x.Symbol);
            foreach (IGrouping<string, Entities.PriceHistory> priceHistoryGrouping in priceHistoryGroupings)
            {
                Dictionary<Entities.PriceHistory, decimal> averages = new Dictionary<Entities.PriceHistory, decimal>();
                foreach (Entities.PriceHistory priceHistory in priceHistoryGrouping)
                {
                    decimal average = (priceHistory.Low + priceHistory.High) / 2;
                    averages.Add(priceHistory, average);
                }

                dailyAverages.Add(priceHistoryGrouping.Key, averages);
            }

            foreach (KeyValuePair<string, Dictionary<Entities.PriceHistory, decimal>> dailyAverage in dailyAverages)
            {
                decimal min = dailyAverage.Value.Min(x => x.Value);
                Entities.PriceHistory priceHistoryMin = dailyAverage.Value.Where(x => x.Value == min).Last().Key;
                decimal max = dailyAverage.Value.Max(x => x.Value);
                Entities.PriceHistory priceHistoryMax = dailyAverage.Value.Where(x => x.Value == max).Last().Key;
                decimal diff = Math.Abs(min / max);
                int daysHighLow = priceHistoryMax.DateTime.Subtract(priceHistoryMin.DateTime).Days;
                int daysLast = priceHistoryMax.DateTime > priceHistoryMin.DateTime ? DateTime.Now.Subtract(priceHistoryMax.DateTime).Days : DateTime.Now.Subtract(priceHistoryMin.DateTime).Days;
                Entities.PriceHistory priceHistoryLast = dailyAverage.Value.Keys.Last();
                PriceHistoryDifferential priceHistoryDifferential = new PriceHistoryDifferential()
                {
                    Symbol = dailyAverage.Key,
                    LastPriceClose = priceHistoryLast.Close,
                    AveragePriceHigh = max,
                    AveragePriceLow = min,
                    Differential = diff,
                    DaysLapsedHighLow = daysHighLow,
                    DaysLapsedLast = daysLast
                };
                priceHistoryDifferentials.Add(priceHistoryDifferential);
            }
            
            /*
            IEnumerable priceHistoryDifferential = priceHistoryDifferentials.Select(x => new
            {
                x.Symbol,
                x.Differential,
                AveragePriceHigh = x.AveragePriceHigh.ToString("C"),
                AveragePriceLow = x.AveragePriceLow.ToString("C"),
                LastPriceClose = x.LastPriceClose.ToString("C"),
                x.DaysLapsedLast,
                x.DaysLapsedHighLow
            }).OrderBy(x => x.Differential).ThenBy(x => x.LastPriceClose);
            */
            return priceHistoryDifferentials;
        }
    }
}
