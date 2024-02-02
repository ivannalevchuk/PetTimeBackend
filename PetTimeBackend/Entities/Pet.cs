namespace PetTimeBackend.Entities
{
    public enum Sex : ushort
    {
        Male = 0,
        Female = 1
    }

    public class Pet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public float Weight { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public Breed Breed { get; set; }
        public long BreedId { get; set; }

    }
}
