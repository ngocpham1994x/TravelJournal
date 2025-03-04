using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelJournal.Data;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GeoService _geoService;
        private readonly WeatherService _weatherService;

        public LocationsController(ApplicationDbContext context, GeoService geoService, WeatherService weatherService)
        {
            _context = context;
            _geoService = geoService;
            _weatherService = weatherService;
        }

        // GET: Locations
        public async Task<IActionResult> Index(string sortOrder)
        {
            var locations = await _context.Location.Include(location => location.City).ToListAsync();

            // implementing Sort feature on City and Country
            ViewData["cityOrder"] = string.IsNullOrEmpty(sortOrder) ? "city_desc" : "";
            ViewData["countryOrder"] = (sortOrder == "country_asc") ? "country_desc" : "country_asc";

            switch (sortOrder)
            {
                case "city_desc":
                    locations = locations.OrderByDescending(l => l.City.CityName).ToList();
                    break;

                case "country_desc":
                    locations = locations.OrderByDescending(l => l.City.CountryName).ToList();
                    break;

                case "country_asc":
                    locations = locations.OrderBy(l => l.City.CountryName).ToList();
                    break;

                default:
                    locations = locations.OrderBy(l => l.City.CityName).ToList();
                    break;
            }

            return View(locations);
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            // Fetch the temperature using OpenWeather API
            float temperature = await _weatherService.GetWeatherAsync(location.Lat, location.Lon);

            // Pass temperature to the view
            ViewData["Temperature"] = float.IsNaN(temperature) ? "Unavailable" : $"{temperature}°C";


            return View(location);
        }

        // GET: Locations/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PlaceName,Address,CityName,CountryName,Lat,Lon,DateVisit,TimeVisit")] Location location)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.Location.Any(l => l.Address == location.Address && l.CityName == location.CityName && l.CountryName == location.CountryName);

                if (isDuplicate)
                {
                    ModelState.AddModelError("", "A location with this address, city, country already exists.");
                    return View(location);
                }

                // Check if the city already exists
                var isCityExist = await _context.City
                    .FirstOrDefaultAsync(city => city.CityName == location.CityName && city.CountryName == location.CountryName);

                if (isCityExist == null)
                {
                    // Fetch latitude and longitude from API
                    var cityWithGeoData = await _geoService.GetCityGeoAsync(location.CityName, location.CountryName);

                    // If city does not exist, create a new city entry with lat/lon from API
                    isCityExist = new City
                    {
                        CityName = location.CityName,
                        CountryName = location.CountryName,
                        Lat = cityWithGeoData.Lat,
                        Lon = cityWithGeoData.Lon
                    };

                    _context.City.Add(isCityExist);
                    await _context.SaveChangesAsync();
                }

                // Associate location with the city
                location.City = isCityExist;

                // fetch lat/lon from API response
                Location geometry = await _geoService.GetLocationGeoAsync(location.Address, location.CityName, location.CountryName);
                if (geometry != null)
                {
                    // Set the latitude and longitude values from the API response
                    location.Lat = geometry.Lat;
                    location.Lon = geometry.Lon;
                }


                _context.Location.Add(location);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            
            return View(location);
        }

        // GET: Locations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaceName,Address,CityName,CountryName,Lat,Lon,DateVisit,TimeVisit")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the city already exists
                    var isCityExist = await _context.City
                        .FirstOrDefaultAsync(city => city.CityName == location.CityName && city.CountryName == location.CountryName);

                    // If the city does not exist, create a new one
                    if (isCityExist == null)
                    {
                        // Fetch latitude and longitude from API
                        var cityWithGeoData = await _geoService.GetCityGeoAsync(location.CityName, location.CountryName);

                        // If city does not exist, create a new city entry with lat/lon from API
                        isCityExist = new City
                        {
                            CityName = location.CityName,
                            CountryName = location.CountryName,
                            Lat = cityWithGeoData.Lat,
                            Lon = cityWithGeoData.Lon
                        };

                        _context.City.Add(isCityExist);
                        await _context.SaveChangesAsync(); // Save new city before updating location
                    }

                    // Associate location with the city
                    location.City = isCityExist;

                    // fetch lat/lon from API response
                    Location geometry = await _geoService.GetLocationGeoAsync(location.Address, location.CityName, location.CountryName);
                    if (geometry != null)
                    {
                        // Set the latitude and longitude values from the API response
                        location.Lat = geometry.Lat;
                        location.Lon = geometry.Lon;
                    }

                    // Update the location
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.Id == id);
        }

        // GET: for cshtml Cities/Edit
        public async Task<JsonResult> GetGeoJson(string address, string cityName, string countryName)
        {
            Location geometry = await _geoService.GetLocationGeoAsync(address, cityName, countryName);

            return Json(new { lat = geometry.Lat, lon = geometry.Lon });  // Return as JSON
        }

    }
}
