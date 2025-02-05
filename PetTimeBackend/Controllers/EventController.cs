using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.IServices;
using PetTimeBackend.Services;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public IActionResult CreateEvent([FromBody] EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _eventService.CreateEvent(model);
                return Ok(new { Message = "Event added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("pet/{petId}")]
        public IActionResult GetEventsByPetId(long petId)
        {
            try
            {
                var petEvents = _eventService.GetEventsByPetId(petId);

                if (petEvents == null || !petEvents.Any())
                {
                    return NotFound("No events found for the specified pet");
                }

                return Ok(petEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet ("events/{date}")]
        public IActionResult GetEventsByDate(string date)
        {
            try
            {
                var dateEvents = _eventService.GetEventsByDate(date);

                if (dateEvents == null || !dateEvents.Any())
                {
                    return Ok(new List<Event>());
                }

                return Ok(dateEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult> DeletePet(long id)
        {
            await _eventService.RemoveEvent(id);
            return NoContent();
        }


    }
}
