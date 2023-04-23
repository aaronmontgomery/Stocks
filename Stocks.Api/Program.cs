using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Stocks.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders();
                    logBuilder.AddDebug();
                    logBuilder.AddConsole();
                    logBuilder.AddEventLog(configure =>
                    {
                        configure.SourceName = "Stocks.Api";
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
