using GoogleApi.Entities.Maps.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Entities;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccinationController(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        [HttpPost]
        public IActionResult CreateVaccination([FromBody] VaccinationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _vaccinationService.CreateVaccination(model);
                return Ok(new { Message = "Vaccination added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVaccination(long id, [FromBody] VaccinationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                model.Id = id;
                _vaccinationService.UpdateVaccination(model);
                return Ok(new { Message = "Vaccination updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVaccination(long id)
        {
            try
            {
                _vaccinationService.DeleteVaccination(id);
                return Ok(new { Message = "Vaccination deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("pet/{petId}")]
        public IActionResult GetVaccinationsByPetId(long petId)
        {
            try
            {
                var vaccinations = _vaccinationService.GetVaccinationsByPetId(petId);
                if (!vaccinations.Any())
                {
                    return NotFound("No vaccinations found for the specified pet");
                }

                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetVaccinationById(long id)
        {
            try
            {
                var vaccination = _vaccinationService.GetVaccinationById(id);
                if (vaccination == null)
                {
                    return NotFound("Vaccination not found in the database");
                }

                return Ok(vaccination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
