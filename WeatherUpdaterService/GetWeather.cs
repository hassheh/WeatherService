using Newtonsoft.Json;
using Quartz;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace WeatherService.WeatherUpdaterService
{
    public class GetWeather : IJob 
    {
        public async Task<List<Root>> Get()
        {
            var weatherModels = new List<Root>();
            using (var client = GetHttpClient())
            {
                try
                {
                    var appId = "6e630221f0656c28f09b1bc7c217eea2";
                    var cities = "espoo,vaasa";
                    var weatherCities = cities.Split(',');
                    var forcasteDays = 5;

                    foreach (var city in weatherCities)
                    {
                        HttpResponseMessage response = await client.GetAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&cnt={1}&appid={2}", city, forcasteDays, appId));
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            return default;
                        }
                        string json = response.Content.ReadAsStringAsync().Result;
                        var weathermodel = JsonConvert.DeserializeObject<Root>(json);
                        weatherModels.Add(weathermodel);
                    }
                }
                catch (Exception ex)
                {
                    var messaage = ex;
                }

                return weatherModels;
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Get();
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
