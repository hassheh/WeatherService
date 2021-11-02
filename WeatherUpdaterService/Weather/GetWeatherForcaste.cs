using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;
using WeatherService.WeatherUpdaterService.Helper;

namespace WeatherService.WeatherUpdaterService.Weather
{
    public class GetWeatherForcaste : IGetWeatherForcaste
    {
        public async Task<List<Root>> GetWeatherFromThirdParty()
        {
            var weatherModels = new List<Root>();
            using (var client = WeatherHelper.GetHttpClient())
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
                        string json = await response.Content.ReadAsStringAsync();
                        var weatherModel = JsonConvert.DeserializeObject<Root>(json);
                        weatherModel.list.ForEach(w =>
                        {
                            if (WeatherHelper.ConvertKelvinToClesius(w.main.temp) < 10)
                                w.LimitExceeds = true;
                            else
                                w.LimitExceeds = false;
                        });

                        weatherModels.Add(weatherModel);
                    }
                }
                catch (Exception ex)
                {
                    var messaage = ex;
                }

                return weatherModels;
            }
        }
    }
}
