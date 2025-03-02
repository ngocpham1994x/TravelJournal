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
    }
}
