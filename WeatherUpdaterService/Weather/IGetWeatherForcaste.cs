using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService.Weather
{
    public interface IGetWeatherForcaste
    {
        Task<List<Root>> GetWeatherFromThirdParty();
        void GetAndTransformAndSaveWeather(object state);
    }
}