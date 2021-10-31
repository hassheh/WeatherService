using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherService.Models
{
    public class coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
    }

    public class wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class clouds
    {
        public int all { get; set; }
    }

    public class sys
    {
        public int type   { get; set; }
        public long id { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    public class WeatherInfo
    {
        public coord coord { get; set; }
        public weather weather { get; set; }
        public string basee { get; set; }
        public main main { get; set; }
        public int visibility { get; set; }
        public wind wind { get; set; }
        public clouds cloud { get; set; }
        public long dt { get; set; }
        public sys sys { get; set; }
        public long timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}
