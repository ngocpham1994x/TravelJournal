using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelJournal.Models
{
    public class Location
    {
        public int Id { get; set; }
        [AllowNull]
        public string PlaceName { get; set; }
        [AllowNull]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter a city")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Please enter a country")]
        public string CountryName { get; set; }
        [AllowNull]
        public float Lat { get; set; }
        [AllowNull]
        public float Lon { get; set; }
        [AllowNull]
        public DateOnly DateVisite { get; set; }
        [AllowNull]
        public TimeOnly TimeVisit { get; set; }

        // Navigation property
        [ValidateNever]
        public City City{ get; set; }   

        public Location()
        {

        }
    }
}
