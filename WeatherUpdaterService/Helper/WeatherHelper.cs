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
        public static double ConvertKelvinToClesius(double kelvinTemp)
        {
            return 273.15 - kelvinTemp;
        }

        public static DateTime ConvertUnixToDateTime(long unixTime)
        {
            DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            return dateTimeOffSet.DateTime;
        }
    } 
}
