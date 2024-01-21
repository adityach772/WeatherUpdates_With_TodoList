using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoList.Models;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    

    public WeatherService(HttpClient httpClient,IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<WeatherResDisplay>> GetTemperatureAsync(double latitude, double longitude)
    {
        string _apiKey = _configuration.GetValue<string>("MyKeys:Weather_API_Key");
        string url = $"https://api.openweathermap.org/data/3.0/onecall?lat={latitude}&lon={longitude}&units=metric&exclude=current,minutely,daily,alerts&appid={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return ParseWeatherApiResponse(content);     
             
        }
        return new List<WeatherResDisplay>();
    }

    private List<WeatherResDisplay> ParseWeatherApiResponse(string response)

    {
        var received_data = JsonConvert.DeserializeObject<WeatherApiResponse>(response);

        List<WeatherResDisplay> weatherDisplays = received_data.Hourly.Select(hourlyData =>
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(hourlyData.Dt).ToLocalTime();
            return new WeatherResDisplay
            {
                Date = dateTime.Date.ToShortDateString(),
                Time = dateTime.ToString("hh:mm tt"),
                Temperature = hourlyData.Temp,
                FeelsLike = hourlyData.FeelsLike,
                WeatherMain = hourlyData.WeatherDetails.FirstOrDefault()?.Main,
                WeatherDescription = hourlyData.WeatherDetails.FirstOrDefault()?.Description
            };
        }).ToList();
        return weatherDisplays;



        // Implement logic to parse the Weather API response and extract required info

        // var jsonObject = JObject.Parse(response);
        //var location = jsonObject["results"]?[0]?["geometry"]?["location"];
        //var temperature = jsonObject["main"]?["temp"]?.Value<double>();

    }
}
