using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService.Weather
{
    public interface IGetWeatherForcast
    {
        Task<List<Root>> GetWeatherFromThirdParty();
        void GetAndTransformAndSaveWeather(object state);
    }
}