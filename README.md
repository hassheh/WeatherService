# WeatherService
This service reads the weather forcast for next 5 days from https://openweathermap.org/api after every 15 min (Configurable from appSettings.json). 
The data is saved to Azure SQL server with indication if the temperature limit exceeds (limit given in appSettings.json).

# How to configure:

The following values could be changed in the appsettings.json to configure the Weathure service 
WeatherCities: Provide a comma seperated list of cities to fetch the weather for these cities.
ForcastDays: These many days of weather forcast will fetched.
WeatherLimitCelsius: The temperature in Celsius to compare against the forcast temperature. 
WeatherCallingFrequencyInMinutes: The open weather API will be called after this many minutes.

# How to run
Run command "dotnet run" in PowerShell in project directory.
Some package issues could be resolved by the command "dotnet restore".

