using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService
{
    public class GetWeather : IJob
    {
        public async Task<WeatherInfo>Get()
        {
            var client = new RestClient("api.openweathermap.org/data/2.5/weather?q=tampere,fi&appid=6e630221f0656c28f09b1bc7c217eea2");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            //if (response.IsSuccessful)
            return JsonConvert.DeserializeObject<WeatherInfo>(response.Content);
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Get();
        }
    }
}
