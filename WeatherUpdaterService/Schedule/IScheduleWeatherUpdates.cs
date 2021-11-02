using Quartz;

namespace WeatherService.WeatherUpdater.Schedule
{
    interface IScheduleWeatherUpdates
    {
        ITrigger GetTriggerWithInterval(int interval);
        void CreateAndRunTask();
        IJobDetail CreateWeatherJob(string jobKey);
    }
}
