using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WeatherService.Models;
using WeatherService.WeatherUpdaterService.BusinessLogic;
using WeatherService.WeatherUpdaterService.Helper;

namespace WeatherService.WeatherUpdaterService
{
    public class SaveWeather : ISaveWeather
    {
        private readonly IWeatherLogic _weatherLogic;

        public SaveWeather(IWeatherLogic _weatherLogic)
        {
            this._weatherLogic = _weatherLogic;
        }

        public void TransformAndStoreWeatherResults(List<Root> thirdPartyWeatherData)
        {
            var weatherDate = TransformData(thirdPartyWeatherData);
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "weather-service-server.database.windows.net";
                builder.UserID = "sqlWeatherServiceLogin";
                builder.Password = "O%1V3STLZin##N";
                builder.InitialCatalog = "WeatherInfo";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT name, collation_name FROM sys.databases";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private List<WeatherData> TransformData(List<Root> thirdPartyWeatherData)
        {
            List<WeatherData> weatherData = new List<WeatherData>();            
            thirdPartyWeatherData = _weatherLogic.MarkWeatherLimits(thirdPartyWeatherData);

            thirdPartyWeatherData.ForEach(data => 
            {
                data.list.ForEach(list =>
                {
                    weatherData.Add(
                    new WeatherData()
                    {
                        CityName = data.city.name,
                        Country = data.city.country,
                        Timezone = data.city.timezone,
                        Description = list.weather.FirstOrDefault().description,
                        Temperature = list.main.temp,
                        TemperatureMin = list.main.temp_min,
                        TemperatureMax = list.main.temp_max,
                        FeelsLike = list.main.feels_like,
                        ExceedsLimits = list.LimitExceeds,
                        WeatherDate = WeatherHelper.ConvertUnixToDateTime(list.dt)
                    });
                });
            });

            return weatherData;
        }
    }
}