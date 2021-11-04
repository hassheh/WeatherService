using System;

namespace WeatherService.WeatherUpdaterService.Helper
{
    public class WeatherHelper
    {
        public WeatherHelper()
        {
        }

        public static double ConvertKelvinToClesius(double? kelvinTemp)
        {
            return 273.15 - kelvinTemp.Value;
        }

        public static DateTime ConvertUnixToDateTime(long? unixTime)
        {
            DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeSeconds(unixTime.Value);
            return dateTimeOffSet.DateTime;
        }
    } 
}
