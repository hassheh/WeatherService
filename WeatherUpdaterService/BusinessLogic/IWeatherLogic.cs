using System.Collections.Generic;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService.BusinessLogic
{
    public interface IWeatherLogic
    {
        List<Root> MarkWeatherLimits(List<Root> thirdPartyWeatherData);
        List<WeatherData> TransformData(List<Root> thirdPartyWeatherData);
    }
}