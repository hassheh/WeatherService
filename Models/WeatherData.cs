using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherService.Models
{
    public class WeatherData
    {
        public string name { get; set; }
        public string country { get; set; }
        public int timezone { get; set; }
        public string description { get; set; }
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double feels_like { get; set; }
        public bool ExceedsLimits { get; set; }
    }
}
