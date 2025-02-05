using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PetTimeBackend.Entities
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ImageUrl { get; set; }
        public long CityId { get; set; }
        public List<Pet> Pets { get; set; }

        public City City { get; set; }
    }
}
