using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherService.WeatherUpdaterService.Weather;

namespace WeatherService.WeatherUpdaterService.Schedule
{
    public class ScheduleWeatherUpdate : IScheduleWeatherUpdate
    {
        private Timer timer;
        private readonly IGetWeatherForcaste igetWeatherForcaste;

        public ScheduleWeatherUpdate(IGetWeatherForcaste igetWeatherForcaste)
        {
            this.igetWeatherForcaste = igetWeatherForcaste;
        }

        public void StartAsync()
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));
        }

        private void DoWork(object state)
        {
            try
            {
                igetWeatherForcaste.GetWeatherFromThirdParty();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                if (ex.InnerException != null && ex.InnerException.Message == null)
                    Console.WriteLine("InnerEx: " + ex.InnerException);

                throw;
            }
        }

        public void StopAsync()
        {
            timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}



