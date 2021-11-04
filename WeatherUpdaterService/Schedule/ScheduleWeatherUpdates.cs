using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherService.WeatherUpdaterService.Weather;

namespace WeatherService.WeatherUpdaterService.Schedule
{
    public class ScheduleWeatherUpdate : IScheduleWeatherUpdate
    {
        private Timer timer;
        private readonly IGetWeatherForcaste igetWeatherForcaste;
        private readonly IConfiguration _configuration;

        public ScheduleWeatherUpdate(IGetWeatherForcaste igetWeatherForcaste, IConfiguration _configuration)
        {
            this.igetWeatherForcaste = igetWeatherForcaste;
            this._configuration = _configuration;
        }

        public void StartAsync()
        {
            var parsed = int.TryParse(_configuration["WeatherServiceConfigs:WeatherCallingFrequencyInMinutes"], out int callingFrequency);
            if (parsed)
                timer = new Timer(igetWeatherForcaste.GetAndTransformAndSaveWeather, null, TimeSpan.Zero,
                    TimeSpan.FromMinutes(callingFrequency));
        }

        public void StopAsync()
        {
            timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}



