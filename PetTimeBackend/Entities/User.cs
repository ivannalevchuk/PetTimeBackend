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
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public List<Pet> Pets { get; set; }
    }
}
