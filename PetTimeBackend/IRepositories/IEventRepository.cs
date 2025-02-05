using PetTimeBackend.Entities;

namespace PetTimeBackend.IRepositories
{
    public interface IEventRepository
    {
        Event GetEventById(long eventId);
        IEnumerable<Event> GetEventsByPetId(long petId);
        void AddEvent(Event newEvent);
        Task RemoveEvent(long eventId);
        Pet GetPetById(long petId);
        IEnumerable<Event> GetEventsByDate(string date);
        void Save();
    }
}
