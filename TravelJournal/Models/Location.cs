using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelJournal.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Display(Name = "Place Name")]
        public string? PlaceName { get; set; }
        [Required(ErrorMessage = "Please enter an address")]
        public string Address { get; set; }
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
        [Display(Name = "Date Visit")]
        public DateOnly? DateVisit { get; set; }
        [Display(Name = "Time Visit")]
        public TimeOnly? TimeVisit { get; set; }

        // Navigation property
        [ValidateNever]
        public City City{ get; set; }   

        public Location()
        {

        }
    }
}
