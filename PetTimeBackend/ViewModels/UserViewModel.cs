using PetTimeBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace PetTimeBackend.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public long CityId { get; set; }
        public string? Image { get; set; }
    }
}
