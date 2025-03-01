using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelJournal.Models
{
    public class City
    {

        [Required(ErrorMessage = "Please enter a city")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Please enter a country")]
        public string CountryName { get; set; }
        [AllowNull]
        public float Lat { get; set; }
        [AllowNull]
        public float Lon { get; set; }

        // navigation property
        public ICollection<Location> Locations { get; set; }

        public City()
        {

        }
    }
}
