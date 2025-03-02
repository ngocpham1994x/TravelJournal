using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class ApiHelper
    {
        //private string OpenweatherApiKey = "9c36f3c87a50dc04716c816304ac6762";

        public static HttpClient _httpClient { get; private set; }

        public static void InitializeClient ()
        {
            _httpClient = new HttpClient ();
            _httpClient.DefaultRequestHeaders.Accept.Clear ();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        //// Google Map API processing
        //public async Task<City> ApiResponseGeometry(string cityName, string countryName)
        //{
        //    string request = $"https://maps.googleapis.com/maps/api/geocode/json?address={cityName.Replace(' ', '+')},{countryName.Replace(' ', '+')}&key={GoogleApiKey}";

        //    using (HttpResponseMessage response = await _httpClient.GetAsync(request))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonResponse = await response.Content.ReadAsStringAsync();

        //            // Parse the JSON response
        //            JObject json = JObject.Parse(jsonResponse);

        //            // Check if the "results" is not empty
        //            if (json["results"] != null)
        //            {
        //                // Extract location (lon,lat) from the first result
        //                var result = json["results"].FirstOrDefault();
        //                var location = result["geometry"]["location"];

        //                // Ensure location exist
        //                if (location != null && location.HasValues)
        //                {
        //                    // Return the latitude and longitude as part of a City object
        //                    return new City
        //                    {
        //                        CityName = cityName,
        //                        CountryName = countryName,
        //                        Lat = location["lat"]?.Value<float>(),
        //                        Lon = location["lng"]?.Value<float>()
        //                    };
        //                }
        //            }
        //        }
        //    }

        //    return new City
        //    {
        //        CityName = cityName,
        //        CountryName = countryName,
        //        Lat = null, // No valid lat/lon retrieved
        //        Lon = null, // No valid lat/lon retrieved
        //    };

        //}
    
    
    }
}
