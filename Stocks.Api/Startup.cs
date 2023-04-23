using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Stocks.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextFactory<Entities.StocksDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Development"));
            });
            
            services.AddScoped<Entities.IDataAdapter, Entities.DataAdapter>();

            services.AddScoped<Modules.TdAmeritrade.IInstruments, Modules.TdAmeritrade.Instruments>();
            
            services.AddScoped<Modules.TdAmeritrade.IPriceHistory, Modules.TdAmeritrade.PriceHistory>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stocks.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stocks.Api v1"));
            }
            
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
