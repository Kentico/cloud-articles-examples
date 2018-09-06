using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Shared
{
    public class WeatherForecast
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("temperatureF")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
