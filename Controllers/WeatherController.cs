using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly GeoCodingService _geoCodingService;
        private readonly WeatherService _weatherService;

        public WeatherController(IHttpClientFactory httpClientFactory, IConfiguration configuration, GeoCodingService geoCodingService, WeatherService weatherService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _geoCodingService = geoCodingService;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DisplayGeoCodes(Address address)
        {
            try
            {
                var (latitude, longitude) = await _geoCodingService.GetLatitudeLongitudeAsync(address);

                var weatherResDisplay = await _weatherService.GetTemperatureAsync(latitude, longitude);

                weatherResDisplay = weatherResDisplay.OrderBy(w => w.Date).ThenBy(w => w.Time).ToList();

                ViewBag.Latitude = latitude;
                ViewBag.Longitude = longitude;
                ViewBag.Address = address;

                return View("WeatherResDisplay",weatherResDisplay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Display()
        {
            try
            {
                var apiUrl = _configuration.GetValue<string>("MyKeys:Weather_API_URL"); // Replace with the actual API URL

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var response = await httpClient.GetStringAsync(apiUrl);
                    return Ok(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }
    }
}
