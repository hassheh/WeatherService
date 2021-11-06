using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models;
using WeatherService.WeatherUpdaterService.BusinessLogic;

namespace WeatherService.WeatherUpdaterService.Weather
{
    public class GetWeatherForcast : IGetWeatherForcast
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ISaveWeather _saveWeather;
        private readonly IWeatherLogic _weatherLogic;

        public GetWeatherForcast(IHttpClientFactory _clientFactory, IConfiguration _configuration, 
            ISaveWeather _saveWeather, IWeatherLogic _weatherLogic)
        {
            this._clientFactory = _clientFactory;
            this._configuration = _configuration;
            this._saveWeather = _saveWeather;
            this._weatherLogic = _weatherLogic;
        }

        public async void GetAndTransformAndSaveWeather(object state)
        {
            List<WeatherData> weatherModels = new List<WeatherData>();
            try
            {
                var modelFromThirdParty = await GetWeatherFromThirdParty();
                if (modelFromThirdParty.Count > 0)
                {
                    weatherModels = _weatherLogic.TransformData(modelFromThirdParty);
                    if (weatherModels.Count > 0)
                        _saveWeather.StoreWeatherResults(weatherModels);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                if (ex.InnerException != null && ex.InnerException.Message == null)
                    Console.WriteLine("InnerEx: " + ex.InnerException);

                throw;
            }
        }

        public async Task<List<Root>> GetWeatherFromThirdParty()
        {
            var weatherModels = new List<Root>();
            int maxRetries = 10;
            int maxDelayMilliseconds = 2000;
            int delayMilliseconds = 200;
            var backoff = new ExponentialBackoff(delayMilliseconds, maxDelayMilliseconds);

            var cities = _configuration["WeatherServiceConfigs:WeatherCities"];

            var weatherCities = cities.Split(',');
            if (weatherCities.Length > 0)
            {
                foreach (var city in weatherCities)
                {
                    for (var retry = 0; retry < maxRetries; retry++)
                    {
                        try
                        {
                            var response = await MakeAPICall(city);

                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                await backoff.Delay().ConfigureAwait(false);
                            }
                            else
                            {
                                string weatherDataJson = await response.Content.ReadAsStringAsync();
                                var weatherModel = JsonConvert.DeserializeObject<Root>(weatherDataJson);
                                weatherModels.Add(weatherModel);
                                return weatherModels;
                            }
                        }
                        catch (Exception ex)
                        {
                            await backoff.Delay().ConfigureAwait(false);
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return weatherModels; 
        }
        private async Task<HttpResponseMessage> MakeAPICall(string city)
        {
            var forcastDays = int.Parse(_configuration["WeatherServiceConfigs:ForcastDays"]);
            var appId = _configuration["WeatherServiceConfigs:AppId"];

            var uri = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&cnt={1}&appid={2}", city, forcastDays, appId);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }
    }
}
