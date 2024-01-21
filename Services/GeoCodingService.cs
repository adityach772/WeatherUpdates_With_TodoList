using Newtonsoft.Json.Linq;
using System.Net;
using ToDoList.Models;

public class GeoCodingService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GeoCodingService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<(double Latitude, double Longitude)> GetLatitudeLongitudeAsync(Address address)
    {
        try
        {
            string encodedAddress = WebUtility.UrlEncode($"{address.Street} {address.City} {address.State}");
            string api_key = _configuration.GetValue<string>("MyKeys:Maps_API_Key");
            var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={api_key}";

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetStringAsync(apiUrl);
                var result = ParseGeocodingResponse(response);

                return result;
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle exception as needed
            throw new Exception($"Geocoding error: {ex.Message}", ex);
        }
    }

    private (double Latitude, double Longitude) ParseGeocodingResponse(string response)
    {
        // Implement logic to parse the Geocoding API response and extract latitude and longitude
        // For simplicity, let's assume a straightforward parsing here

        // Example: Extracting latitude and longitude from a JSON response
        var jsonObject = JObject.Parse(response);
        var location = jsonObject["results"]?[0]?["geometry"]?["location"];
        double latitude = location?["lat"]?.Value<double>() ?? 0.0;
        double longitude = location?["lng"]?.Value<double>() ?? 0.0;

        return (latitude, longitude);
    }
}
