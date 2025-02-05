namespace PetTimeBackend.Entities
{
    public class Vaccination
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }
        public long PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
