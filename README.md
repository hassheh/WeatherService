# WeatherService
This service reads the weather forcast for next 5 days from https://openweathermap.org/api after every 15 min (Configurable from appSettings.json). 
The data is saved to Azure SQL server with indication if the temperature limit exceeds (limit given in appSettings.json).

#How to run
Run command "Dotnet run" in PowerShell in project directory.
Some package issues could be resolved by the command "Dotnet restore".

