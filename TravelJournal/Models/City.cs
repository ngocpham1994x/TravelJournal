using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelJournal.Models
{
    public class City
    {

        [Required(ErrorMessage = "Please enter a city")]
        [Display(Name = "City")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Please enter a country")]
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        [Display(Name = "Latitude")]
        public float? Lat { get; set; }
        [Display(Name = "Longitude")]
        public float? Lon { get; set; }

        // navigation property
        public ICollection<Location> Locations { get; set; } = new List<Location>();  // Always initialized

        public City()
        {

        }
    }
}
