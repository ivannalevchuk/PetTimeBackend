namespace PetTimeBackend.Entities
{
    public enum EventType
    {
        Competition = 0,
        VetAppointment = 1,
        Training = 2
    }

    public class Event
    {
        public long Id { get; set; }
        public Pet Pet { get; set; }
        public long PetId { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
    }
}
