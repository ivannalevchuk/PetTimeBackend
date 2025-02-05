using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;

namespace PetTimeBackend.IRepositories
{
    public class EventRepository: IEventRepository
    {
        private readonly PetTimeContext _context;

        public EventRepository(PetTimeContext context)
        {
            _context = context;
        }

        public Event GetEventById(long eventId)
        {
            return _context.Events.Find(eventId);
        }

        public IEnumerable<Event> GetEventsByPetId(long petId)
        {
            return _context.Events.Where(e => e.PetId == petId).ToList();
        }

        public void AddEvent(Event newEvent)
        {
            _context.Events.Add(newEvent);
        }

        public async Task RemoveEvent(long id)
        {
            var eventPet = await _context.Events.FindAsync(id);
            if (eventPet != null)
            {
                _context.Events.Remove(eventPet);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Event> GetEventsByDate(string date) {
           return _context.Events.Where(e => e.EventDate.Date == DateTime.Parse(date).Date)
                .ToList();
        }

        public Pet GetPetById(long petId)
        {
            return _context.Pets.Find(petId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
