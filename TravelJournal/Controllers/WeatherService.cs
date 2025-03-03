using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _openweatherApiKey;

        public WeatherService(IConfiguration configuration)
        {
            _httpClient = ApiHelper._httpClient; // Use the global HttpClient instance
            _openweatherApiKey = configuration["OpenWeatherApiKey"]; // Read API Key from appsettings.json
        }

        public string GetOpenWeatherApiKey()
        {
            return _openweatherApiKey;
        }

        // openweather API processing
        public async Task<float> GetWeatherAsync(float? latitude, float? longitude)
        {
            string request = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={_openweatherApiKey}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JObject json = JObject.Parse(jsonResponse);

                    return (float)(json["main"]["temp"]?.Value<float>());
                }
            }

            return float.NaN;
        }
    }
}