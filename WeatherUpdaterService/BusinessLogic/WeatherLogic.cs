using Microsoft.Extensions.Configuration;
using System;
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
                root?.list?.ForEach(list =>
                {
                    if (WeatherHelper.ConvertKelvinToClesius(list?.main?.temp) != weatherLimitCelsius)
                        list.LimitExceeds = true;
                    else
                        list.LimitExceeds = false;
                });
            });

            return thirdPartyWeatherData;
        }

        public List<WeatherData> TransformData(List<Root> thirdPartyWeatherData)
        {
            List<WeatherData> weatherData = new List<WeatherData>();
            thirdPartyWeatherData = MarkWeatherLimits(thirdPartyWeatherData);

            thirdPartyWeatherData.ForEach(data =>
            {
                data?.list?.ForEach(list =>
                {
                    weatherData.Add(
                    new WeatherData()
                    {
                        CityName = data?.city?.name,
                        Country = data?.city?.country,
                        Timezone = data?.city?.timezone ?? 0,
                        Description = list?.weather?.FirstOrDefault()?.description,
                        Temperature = WeatherHelper.ConvertKelvinToClesius(list?.main?.temp),
                        TemperatureMin = WeatherHelper.ConvertKelvinToClesius(list?.main?.temp_min),
                        TemperatureMax = WeatherHelper.ConvertKelvinToClesius(list?.main?.temp_max),
                        FeelsLike = WeatherHelper.ConvertKelvinToClesius(list?.main?.feels_like),
                        ExceedsLimits = list?.LimitExceeds ?? false,
                        WeatherDate = WeatherHelper.ConvertUnixToDateTime(list?.dt)
                    });
                });
            });

            return weatherData;
        }
    }
}
