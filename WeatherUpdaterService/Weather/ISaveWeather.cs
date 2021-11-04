using System.Collections.Generic;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService
{
    public interface ISaveWeather
    {
        void StoreWeatherResults(List<WeatherData> weatherData);
    }
}