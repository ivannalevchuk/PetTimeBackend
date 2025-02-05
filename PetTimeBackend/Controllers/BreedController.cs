using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedController : ControllerBase
    {
        private readonly PetTimeContext _dbContext;
        public BreedController(PetTimeContext context, IConfiguration configuration)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Breed>> GetAllBreedsAsync()
        {
            return await _dbContext.Breeds.ToListAsync();
        }

        [HttpGet("{id}")]
        public async  Task<IActionResult> GetBreedById(long id)
        {
            var breed =   _dbContext.Breeds.Find(id);

            return Ok (breed);
        }


    }
}
