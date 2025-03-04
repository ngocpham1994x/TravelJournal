using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TravelJournal.Data;
using TravelJournal.Models;

namespace TravelJournal.Controllers
{
    public class CitiesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly GeoService _geoService;
        private readonly WeatherService _weatherService;

        public CitiesController(ApplicationDbContext context, GeoService geoService, WeatherService weatherService)
        {
            _context = context;
            _geoService = geoService;
            _weatherService = weatherService;
        }

        // GET: Cities
        public async Task<IActionResult> Index(string sortOrder)
        {
            var cities = await _context.City.ToListAsync();

            ViewData["cityOrder"] = string.IsNullOrEmpty(sortOrder) ? "city_desc" : "";
            ViewData["countryOrder"] = (sortOrder == "country_asc") ? "country_desc" : "country_asc";

            switch (sortOrder)
            {
                case "city_desc":
                    cities = cities.OrderByDescending(c => c.CityName).ToList();
                    break;

                case "country_desc":
                    cities = cities.OrderByDescending(c => c.CountryName).ToList();
                    break;

                case "country_asc":
                    cities = cities.OrderBy(c => c.CountryName).ToList();
                    break;

                default:
                    cities = cities.OrderBy(c => c.CityName).ToList();
                    break;
            }

            return View(cities);
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(string cityName, string countryName)
        {
            if (cityName == null || countryName == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(c => c.CityName == cityName && c.CountryName == countryName);
            if (city == null)
            {
                return NotFound();
            }

            // Fetch the temperature using OpenWeather API
            float temperature = await _weatherService.GetWeatherAsync(city.Lat, city.Lon);
            ViewData["GoogleMapsApiKey"] = _geoService.GetGoogleMapsApiKey(); // Pass API key to View

            // Pass temperature to the view
            ViewData["Temperature"] = float.IsNaN(temperature) ? "Unavailable" : $"{temperature}°C";

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CityName,CountryName,Lat,Lon")] City city)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.City.Any(c => c.CityName == city.CityName && c.CountryName == city.CountryName);

                if (isDuplicate)
                {
                    ModelState.AddModelError("", "A city with this name already exists in the selected country.");
                    return View(city);
                }

                // fetch lat/lon from API response
                City geometry = await _geoService.GetCityGeoAsync(city.CityName, city.CountryName);
                if (geometry != null)
                {
                    // Set the latitude and longitude values from the API response
                    city.Lat = geometry.Lat;
                    city.Lon = geometry.Lon;
                }

                // Add the city to the database
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(city);
        }

        // GET: Cities/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string cityName, string countryName)
        {
            if (cityName == null || countryName == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(cityName, countryName);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string cityName, string countryName, [Bind("CityName,CountryName,Lat,Lon")] City city)
        {
            if (cityName != city.CityName || countryName != city.CountryName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the city
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityName, city.CountryName))
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
            return View(city);
        }

        // GET: Cities/Delete/51
        [Authorize]
        public async Task<IActionResult> Delete(string cityName, string countryName)
        {
            if (cityName == null || countryName == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.CityName == cityName && m.CountryName == countryName);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string cityName, string countryName)
        {
            var city = await _context.City.FindAsync(cityName, countryName);

            if (city != null)
            {
                _context.City.Remove(city);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(string cityName, string countryName)
        {
            return _context.City.Any(e => e.CityName == cityName && e.CountryName == countryName);
        }

        // GET: for cshtml Cities/Edit
        public async Task<JsonResult> GetGeoJson(string cityName, string countryName)
        {
            City geometry = await _geoService.GetCityGeoAsync(cityName, countryName);

            return Json(new {lat = geometry.Lat, lon = geometry.Lon});  // Return as JSON
        }
    }
}
