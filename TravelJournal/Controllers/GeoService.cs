using Newtonsoft.Json.Linq;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class GeoService
    {
        private string GoogleApiKey = "AIzaSyA7JaM35ulMiCDEpnnncTAIPMA9o8OXYzQ";
        private readonly HttpClient _httpClient;

        public GeoService()
        {
            _httpClient = ApiHelper._httpClient; // Use the global HttpClient instance
        }

        // Google Map API processing
        public async Task<City> GetCityGeoAsync(string cityName, string countryName)
        {
            string request = $"https://maps.googleapis.com/maps/api/geocode/json?address={cityName.Replace(' ', '+')},{countryName.Replace(' ', '+')}&key={GoogleApiKey}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JObject json = JObject.Parse(jsonResponse);

                    // Check if the "results" is not empty
                    if (json["results"] != null)
                    {
                        // Extract location (lon,lat) from the first result
                        var result = json["results"].FirstOrDefault();
                        var location = result["geometry"]["location"];

                        // Ensure location exist
                        if (location != null && location.HasValues)
                        {
                            // Return the latitude and longitude as part of a City object
                            return new City
                            {
                                CityName = cityName,
                                CountryName = countryName,
                                Lat = location["lat"]?.Value<float>(),
                                Lon = location["lng"]?.Value<float>()
                            };
                        }
                    }
                }
            }
            return null;
        }
    
        public async Task<Location> GetLocationGeoAsync(string address, string cityName, string countryName)
        {
            string request = $"https://maps.googleapis.com/maps/api/geocode/json?address={address.Replace(' ','+')},{cityName.Replace(' ', '+')},{countryName.Replace(' ', '+')}&key={GoogleApiKey}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JObject json = JObject.Parse(jsonResponse);

                    // Check if the "results" is not empty
                    if (json["results"] != null)
                    {
                        // Extract location (lon,lat) from the first result
                        var result = json["results"].FirstOrDefault();
                        var location = result["geometry"]["location"];

                        // Ensure location exist
                        if (location != null && location.HasValues)
                        {
                            // Return the latitude and longitude as part of a City object
                            return new Location
                            {
                                Address = address,
                                CityName = cityName,
                                CountryName = countryName,
                                Lat = location["lat"]?.Value<float>(),
                                Lon = location["lng"]?.Value<float>()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
