using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using WeatherService.WeatherUpdaterService.Weather;

namespace WeatherService.WeatherUpdaterService.Schedule
{
    public class ScheduleWeatherUpdate : IScheduleWeatherUpdate
    {
        private Timer timer;
        private readonly IGetWeatherForcast igetWeatherForcast;
        private readonly IConfiguration _configuration;

        public ScheduleWeatherUpdate(IGetWeatherForcast igetWeatherForcast, IConfiguration _configuration)
        {
            this.igetWeatherForcast = igetWeatherForcast;
            this._configuration = _configuration;
        }

        public void StartAsync()
        {
            var parsed = int.TryParse(_configuration["WeatherServiceConfigs:WeatherCallingFrequencyInMinutes"], out int callingFrequency);
            if (parsed)
                timer = new Timer(igetWeatherForcast.GetAndTransformAndSaveWeather, null, TimeSpan.Zero,
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



