using Quartz;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService
{
    public interface IGetWeather
    {
        Task Execute(IJobExecutionContext context);
        Task<WeatherInfo> Get();
    }
}