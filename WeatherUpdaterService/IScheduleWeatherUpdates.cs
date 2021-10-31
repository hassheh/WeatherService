using Quartz;
using System.Threading.Tasks;

namespace WeatherService.WeatherUpdater.Schedule
{
    interface IScheduleWeatherUpdates
    {
        ITrigger GetTriggerWithInterval(int interval);
        void CreateAndRunTask();
        IJobDetail CreateWeatherJob(string jobKey);
    }
}
