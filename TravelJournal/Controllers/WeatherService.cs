using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class WeatherService
    {
        private string OpenweatherApiKey = "9c36f3c87a50dc04716c816304ac6762";
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = ApiHelper._httpClient; // Use the global HttpClient instance
        }

        // openweather API processing
        public async Task<float> GetWeatherAsync(float? latitude, float? longitude)
        {
            string request = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={OpenweatherApiKey}";

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