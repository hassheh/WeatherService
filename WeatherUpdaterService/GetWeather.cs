using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System.Threading.Tasks;
using WeatherService.WeatherUpdaterService.Weather;

namespace WeatherService.WeatherUpdater
{
    public class ContainerJobFactory : PropertySettingJobFactory
    {
        private readonly System.ComponentModel.IContainer container;

        public ContainerJobFactory(IContainer container)
        {
            this.container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = container.GetInstance(bundle.JobDetail.JobType);
            if (ReferenceEquals(job, null))
                return base.NewJob(bundle, scheduler);
            SetObjectProperties(job, bundle.JobDetail.JobDataMap);
            return (IJob)job;
        }
    }
}
