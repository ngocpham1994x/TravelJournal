using System.Net.Http.Headers;

namespace TravelJournal.Controllers
{
    public class ApiHelper
    {
        public static HttpClient _httpClient { get; set; }

        public static void InitializeClient ()
        {
            _httpClient = new HttpClient ();
            _httpClient.DefaultRequestHeaders.Accept.Clear ();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
