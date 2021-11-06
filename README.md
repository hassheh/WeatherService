# WeatherService
This service reads the weather forcast from https://openweathermap.org/api after every 15 min (Configurable from appSettings.json) for next 5 days. 
The data is saved to Azure SQL server indicating if the temperature limit exceeds (limit given in appSettings.json).

#How to run
Run command "Dotnet run" in PowerShell in project directory.
Some package issues could be resolved by the command "Dotnet restore".

