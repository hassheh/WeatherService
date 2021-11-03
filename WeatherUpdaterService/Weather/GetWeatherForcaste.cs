using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService.Weather
{
    public class GetWeatherForcaste : IGetWeatherForcaste
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public GetWeatherForcaste(IHttpClientFactory _clientFactory, IConfiguration _configuration)
        {
            this._clientFactory = _clientFactory;
            this._configuration = _configuration;
        }

        public async Task<List<Root>> GetWeatherFromThirdParty()
        {
            var weatherModels = new List<Root>();
            
            try
            {
                var appId = _configuration["WeatherServiceConfigs:AppId"];
                var cities = _configuration["WeatherServiceConfigs:WeatherCities"];
                var forcasteDays = int.Parse(_configuration["WeatherServiceConfigs:ForcasteDays"]);
                
                var weatherCities = cities.Split(',');
                
                foreach (var city in weatherCities)
                {
                    var uri = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&cnt={1}&appid={2}", city, forcasteDays, appId);

                    var request = new HttpRequestMessage(HttpMethod.Get, uri);
                    var client = _clientFactory.CreateClient();
                    var response = await client.SendAsync(request);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return default;
                    }
                    string weatherDataJson = await response.Content.ReadAsStringAsync();
                    var weatherModel = JsonConvert.DeserializeObject<Root>(weatherDataJson);                    
                    weatherModels.Add(weatherModel);
                }
            }
            catch (HttpRequestException ex)
            {
            }

            return weatherModels;            
        }
    }
}
