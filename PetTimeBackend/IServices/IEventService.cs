using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IServices
{
    public interface IEventService
    {
        Event GetEventById(long eventId);
        IEnumerable<Event> GetEventsByPetId(long petId);
        IEnumerable<Event> GetEventsByDate(string date);
        void CreateEvent(EventViewModel model);
        Task RemoveEvent (long eventId);
    }
}
