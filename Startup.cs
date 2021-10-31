using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherService.Models;
using WeatherService.WeatherUpdater.Schedule;
using WeatherService.WeatherUpdaterService;

namespace WeatherService
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
            services.AddSingleton<IScheduleWeatherUpdates, ScheduleWeatherUpdates>();
            
            services.AddOptions();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var weatherService = new ScheduleWeatherUpdates(configuration);
            lifetime.ApplicationStarted.Register(weatherService.CreateAndRunTask);
            lifetime.ApplicationStopped.Register(weatherService.Stop);

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
        }
    }
}
