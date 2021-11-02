using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WeatherService.WeatherUpdaterService.Helper
{
    public class WeatherHelper
    {
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static double ConvertKelvinToClesius(double kelvinTemp)
        {
            return 273.15 - kelvinTemp;
        }
    } 
}
