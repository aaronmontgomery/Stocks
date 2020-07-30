using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stocks.Data;
using Stocks.Models;

namespace Stocks.Web.Controllers
{
    public class StocksController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["DefaultDateTimeRange"] = new DateTimeRange()
            {
                Start = new DateTime(DateTime.UtcNow.Year, 1, 1),
                End = DateTime.UtcNow
            };
            ViewData["Ticker"] = JsonConvert.SerializeObject(CompiledQueries.Ticker());
            return View();
        }
    }
}
