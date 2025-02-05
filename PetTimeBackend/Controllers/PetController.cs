using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
            private readonly IPetService _petService;

            public PetController(IPetService petService)
            {
                _petService = petService;
            }
            
            [HttpGet]
            public async Task<IActionResult> GetAllPets()
            {
                var pets = await _petService.GetAllPetsAsync();
                return Ok(pets);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetPetById(long id)
            {
                var pet = await _petService.GetPetByIdAsync(id);
                if (pet == null)
                {
                    return NotFound();
                }
                return Ok(pet);
            }

            [HttpPost]
            public async Task<IActionResult> AddPet([FromBody] PetViewModel pet)
            {
                var addedPet = await _petService.AddPetAsync(pet);
                return CreatedAtAction(nameof(GetPetById), new { id = addedPet.Id }, addedPet);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdatePet(long id, [FromBody] PetViewModel updatedPet)
            {
                await _petService.UpdatePetAsync(id, updatedPet);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeletePet(long id)
            {
                await _petService.DeletePetAsync(id);
                return NoContent();
            }
        }
    }
