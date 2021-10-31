using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using WeatherService.WeatherUpdaterService;

namespace WeatherService.WeatherUpdater.Schedule
{
    public class ScheduleWeatherUpdates : IScheduleWeatherUpdates
    {
        private IScheduler _scheduler;
        private readonly IConfiguration _configuration;
        public ScheduleWeatherUpdates(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateAndRunTask()
        {
            var interval = int.Parse(_configuration["WeatherCallingFrequency"]);
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;

            _scheduler.Start().Wait();
            var trigger = GetTriggerWithInterval(interval);
            var weatherJob = CreateWeatherJob("GetWeather");
            _scheduler.ScheduleJob(weatherJob, trigger).Wait();
        }

        public ITrigger GetTriggerWithInterval(int interval)
        {
            string cronExpression = string.Format("0 */{0} * ? * *", interval);
            return TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule(cronExpression)
                .Build();
        }

        public IJobDetail CreateWeatherJob(string jobKey)
        {
            return JobBuilder.Create<GetWeather>().WithIdentity(jobKey).Build();
        }

        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            // give running jobs 30 sec (for example) to stop gracefully
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
            {
                _scheduler = null;
            }
            else
            {
                // jobs didn't exit in timely fashion - log a warning...
            }
        }
    }
}
