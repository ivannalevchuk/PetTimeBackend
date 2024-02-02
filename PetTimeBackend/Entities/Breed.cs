namespace PetTimeBackend.Entities
{
    public class Breed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Pet> Pets { get; set; }
        public List<Article> Articles { get; set; }
    }
}
