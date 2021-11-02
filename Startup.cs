using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherService.WeatherUpdaterService.Weather;
using WeatherService.WeatherUpdaterService.Schedule;

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
            services.AddOptions();
            //services.AddSingleton<IScheduleWeatherUpdates, ScheduleWeatherUpdates>();
            services.AddSingleton<IGetWeatherForcaste, GetWeatherForcaste>();
            services.AddHttpClient();
            services.AddAuthorization();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime, IGetWeatherForcaste igetWeatherForcaste)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var weatherService = new ScheduleWeatherUpdate(igetWeatherForcaste);
            lifetime.ApplicationStarted.Register(weatherService.StartAsync);
            lifetime.ApplicationStopped.Register(weatherService.StopAsync);

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
        }
    }
}
