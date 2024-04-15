using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly PetTimeContext _context;
        public EventController(PetTimeContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult CreateEvent([FromBody] EventViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var pet = _context.Pets.FirstOrDefault(p => p.Id == model.PetId) ;
                 if(pet == null)
                {
                    return NotFound("Pet not found");
                }
                var newEvent = new Event
                {
                    PetId = model.PetId,
                    EventDate = model.EventDate,
                    EventType = model.EventType,
                    Description = model.Description
                };
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                return Ok(new { Message = "Event added successfully", EventId = newEvent.Id });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
