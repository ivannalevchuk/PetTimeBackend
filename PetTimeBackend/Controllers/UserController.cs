using Azure.Storage.Blobs;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.Services;
using PetTimeBackend.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PetTimeContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly FirestoreService _firestoreService;
        public UserController(PetTimeContext context, IConfiguration configuration, BlobServiceClient blobServiceClient, FirestoreService firestoreService) 
        {
            _dbContext = context;
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
            _firestoreService = firestoreService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task <IActionResult> GetUserById (long id)
        {
            try
            {
                var user = _dbContext.Users.Find(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          

            if (await _dbContext.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return BadRequest(ModelState);
            }

            model.Password = HashPassword(model.Password);
            var user = new User
            {
                Name= model.Name,
                Email = model.Email,
                Password = model.Password,
                CityId = model.CityId,
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var response = new RegistrationResponse
            {
                UserId = user.Id,
                Message = "Registration successful"
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == HashPassword(request.Password) );

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        [HttpPost("{userId}/upload-image")]
        public async Task<IActionResult> UploadImage(long userId, IFormFile image)
        {
            try
            {
                var user = _dbContext.Users.Find(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image uploaded");
                }

                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var containerClient = _blobServiceClient.GetBlobContainerClient("images");
                var blobClient = containerClient.GetBlobClient(imageName);
                using (var stream = image.OpenReadStream())
                {
                     blobClient.Upload(stream, true);
                }

                user.ImageUrl = blobClient.Uri.ToString();
                _dbContext.SaveChanges();

                return Ok("Image uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("{userId}/location")]
        public async Task<IActionResult> UpdateUserLocation(string userId, [FromBody] LocationUpdateModel model)
        {
            try
            {
                await _firestoreService.SaveUserLocationAsync(userId, model.Latitude, model.Longitude);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/location")]
        public async Task<IActionResult> GetUserLocation(long userId)
        {
            try
            {
                var userLocation = await _firestoreService.GetUserLocationAsync(userId);
                if (userLocation == null)
                {
                    return NotFound("User location not found");
                }

                return Ok(userLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{userId}/locations")]
        public async Task<IActionResult> GetUsersLocation(long userId)
        {
            try
            {
                var userLocation = await _firestoreService.GetAllUserLocationsAsync(userId);
                if (userLocation == null)
                {
                    return NotFound("User location not found");
                }

                return Ok(userLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object is null");
            }

            if (_configuration == null)
            {
                throw new ArgumentNullException(nameof(_configuration), "Configuration object is null");
            }
 
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

public class RegistrationResponse
{
    public long UserId { get; set; }
    public string Message { get; set; }
}

public class LocationUpdateModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}