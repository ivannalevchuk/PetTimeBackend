using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.IServices;
using PetTimeBackend.Services;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : ControllerBase
    {
        private readonly FirestoreService _firestoreService;
        private readonly IReportService _reportService;
        private readonly PetTimeContext _petTimeContext;
        public DistanceController(FirestoreService firestoreService, PetTimeContext petTimeContext)
        {
            _firestoreService = firestoreService;
            _petTimeContext= petTimeContext;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateDistance(long userId)
        {
            try
            {
                // Отримати геолокацію користувача з Firebase Firestore
                var userLocation = await _firestoreService.GetUserLocationAsync(userId);
                if (userLocation == null)
                {
                    return NotFound("User location not found");
                }

                double userLat = userLocation.Latitude;
                double userLon = userLocation.Longitude;
                Console.WriteLine(userLat + " " + userLon);

                // Отримати всі мітки з бази даних SQL
                var reports =  _petTimeContext.Reports
                                   .Where(ad => ad.UserId != userId)
                                   .ToList();

                // Обчислити відстань між користувачем та кожною міткою
                var distances = new Dictionary<long, double>();
                foreach (var marker in reports)
                {
                    double distance =  DistanceCalculator.CalculateDistance(userLat, userLon, marker.Latitude, marker.Longitude);
                    double roundedDistance = Math.Round(distance, 2);
                    distances.Add(marker.Id, roundedDistance);
                }

                return Ok(distances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
