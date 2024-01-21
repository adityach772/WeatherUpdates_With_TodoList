using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class WeatherApiResponse
    {
        [JsonProperty("hourly")]
        public List<HourlyData> Hourly { get; set; }

        public class HourlyData
        {
            [JsonProperty("dt")]
            public long Dt { get; set; }

            [JsonProperty("temp")]
            public double Temp { get; set; }

            [JsonProperty("feels_like")]
            public double FeelsLike { get; set; }

            [JsonProperty("weather")]
            public List<WeatherDetail> WeatherDetails { get; set; }

            // Additional method to convert Unix timestamp to DateTime
           
        }

        public class WeatherDetail
        {
            [JsonProperty("main")]
            public string Main { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }
        }
    }

}