using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using WeatherService.Models;
using WeatherService.WeatherUpdaterService.Helper;

namespace WeatherService.WeatherUpdaterService.BusinessLogic
{
    public class WeatherLogic : IWeatherLogic
    {
        private readonly IConfiguration _configuration;
        public WeatherLogic(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public List<Root> MarkWeatherLimits(List<Root> thirdPartyWeatherData)
        {
            int weatherLimitCelsius = int.Parse(_configuration["WeatherServiceConfigs:WeatherLimitCelsius"]);

            thirdPartyWeatherData.ForEach(root =>
            {
                root.list.ForEach(list =>
                {
                    if (WeatherHelper.ConvertKelvinToClesius(list.main.temp) != weatherLimitCelsius)
                        list.LimitExceeds = true;
                    else
                        list.LimitExceeds = false;
                });
            });

            return thirdPartyWeatherData;
        }
    }
}
