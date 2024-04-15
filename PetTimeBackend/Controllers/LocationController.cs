﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using System.Net.Http;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly PetTimeContext _dbContext;
        private readonly HttpClient _httpClient;
        public LocationController(PetTimeContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult GetLocations()
        {
            try
            {
                var locations = _dbContext.Places.ToList();

                if (locations == null || locations.Count == 0)
                {
                    return NotFound("No locations found");
                }

                foreach (var location in locations)
                {
                    string address = GetAddress(location.Latitude, location.Longitude);
                    location.Address = address;
                }

                _dbContext.SaveChanges(); 

                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetLocationById(int id)
        {
            try
            {
                var location = _dbContext.Places.FirstOrDefault(p => p.Id == id);

                if (location == null)
                {
                    return NotFound($"Location with ID {id} not found");
                }

                var locationModel = new Place
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };

                return Ok(locationModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("api/location/getlocationbyname")]
        public async Task<IActionResult> GetLocationByName([FromQuery] string placeName)
        {
            try
            {
                string apiKey = "AIzaSyB0nUlWRWmDS_4EMfIkUjXFcEHMvWbXtTk";
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(placeName)}&key={apiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                double latitude = jsonResponse.results[0].geometry.location.lat;
                double longitude = jsonResponse.results[0].geometry.location.lng;

                return Ok(new { Latitude = latitude, Longitude = longitude });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private string GetAddress(double latitude, double longitude)
        {
            try
            {
                string apiKey = "AIzaSyB0nUlWRWmDS_4EMfIkUjXFcEHMvWbXtTk";
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";

                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string address = jsonResponse.results[0].formatted_address;

                return address;
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddPlace([FromBody] NewPlaceRequest request)
        {
            try
            {
                // Use the Google Maps Geocoding API to get latitude and longitude
                string apiKey = "AIzaSyB0nUlWRWmDS_4EMfIkUjXFcEHMvWbXtTk";
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={request.Address}&key={apiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                float latitude = jsonResponse.results[0].geometry.location.lat;
                float longitude = jsonResponse.results[0].geometry.location.lng;

                // Create a new Place object
                var newPlace = new Place
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Name = request.Name,
                    Address = request.Address,
                    // You can include other properties of Place here
                };

                // Save the new place to the database
                _dbContext.Places.Add(newPlace);
                _dbContext.SaveChanges();

                return Ok(newPlace);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public class NewPlaceRequest
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }

}