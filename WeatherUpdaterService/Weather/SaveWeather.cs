using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WeatherService.Models;

namespace WeatherService.WeatherUpdaterService
{
    public class SaveWeather : ISaveWeather
    {
        private readonly IConfiguration _configuration;
        public SaveWeather(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public void StoreWeatherResults(List<WeatherData> weatherData)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                /*
                
                Azure key vault is setup to save SQL server secrets but it requires
                setting env veriables on host machine. Skipped so the solution could run when provided.
                
                 */
                
                //var keyVaultName = _configuration["KeyVaultName"];
                //var kvUri = "https://" + keyVaultName + ".vault.azure.net";
                //var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

                //var dataSource = await client.GetSecretAsync("DataSource");
                //builder.DataSource = dataSource.Value.ToString();
                //var userID = await client.GetSecretAsync("UserID");
                //builder.UserID = userID.Value.ToString();
                //var password = await client.GetSecretAsync("Password");
                //builder.Password = password.Value.ToString();

                builder.DataSource = "weather-service-server.database.windows.net";
                builder.UserID = "sqlWeatherServiceLogin";
                builder.Password = "O%1V3STLZin##N";
                builder.InitialCatalog = "WeatherInfo";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string saveWeatherDataSQL = @"INSERT INTO Weather(CityName, Country, Timezone, Description, Temperature, TemperatureMin, TemperatureMax, FeelsLike, ExceedsLimits, WeatherDate)
                                                  Values(@CityName, @Country, @Timezone, @Description, @Temperature, @TemperatureMin, @TemperatureMax, @FeelsLike, @ExceedsLimits, @WeatherDate) ";

                    connection.Open();
                    SqlTransaction trans = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(saveWeatherDataSQL, connection, trans))
                    {
                        command.Parameters.Add("@CityName", SqlDbType.NVarChar);
                        command.Parameters.Add("@Country", SqlDbType.NVarChar);
                        command.Parameters.Add("@Timezone", SqlDbType.Int);
                        command.Parameters.Add("@Description", SqlDbType.NVarChar);
                        command.Parameters.Add("@Temperature", SqlDbType.Float);
                        command.Parameters.Add("@TemperatureMin", SqlDbType.Float);
                        command.Parameters.Add("@TemperatureMax", SqlDbType.Float);
                        command.Parameters.Add("@FeelsLike", SqlDbType.Float);
                        command.Parameters.Add("@ExceedsLimits", SqlDbType.Bit);
                        command.Parameters.Add("@WeatherDate", SqlDbType.DateTime);
                       
                        foreach (var weather in weatherData)
                        {
                            command.Parameters[0].Value = weather.CityName;
                            command.Parameters[1].Value = weather.Country;
                            command.Parameters[2].Value = weather.Timezone;
                            command.Parameters[3].Value = weather.Description;
                            command.Parameters[4].Value = weather.Temperature;
                            command.Parameters[5].Value = weather.TemperatureMin;
                            command.Parameters[6].Value = weather.TemperatureMax;
                            command.Parameters[7].Value = weather.FeelsLike;
                            command.Parameters[8].Value = weather.ExceedsLimits;
                            command.Parameters[9].Value = weather.WeatherDate;

                            command.ExecuteNonQuery();
                        }

                        trans.Commit();
                        connection.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}