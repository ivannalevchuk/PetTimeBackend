using PetTimeBackend.Entities;

namespace PetTimeBackend.ViewModels
{
    public class PetViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public float Weight { get; set; }
        public long UserId { get; set; }
        public long BreedId { get; set; }

    }
}
