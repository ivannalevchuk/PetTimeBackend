using PetTimeBackend.Entities;

namespace PetTimeBackend.ViewModels
{
    public class EventViewModel
    {
        public long PetId { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
    }
}
