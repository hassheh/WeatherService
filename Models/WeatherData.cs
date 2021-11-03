using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherService.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public int Timezone { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public double FeelsLike { get; set; }
        public bool ExceedsLimits { get; set; }
        public DateTime WeatherDate { get; set; }
    }
}
