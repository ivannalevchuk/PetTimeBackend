using PetTimeBackend.Entities;

namespace PetTimeBackend.ViewModels
{
    public class ReportViewModel
    {
        //public long Id { get; set; }
        public long UserId { get; set; }
        public string PetName { get; set; }
        public string Breed { get; set; }
        public Sex Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public IFormFile Image { get; set; }

        //public string Image { get; set; }
    }
}
