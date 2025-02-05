namespace PetTimeBackend.Entities
{
   
    public class Report
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string PetName { get; set; }
        public string Breed { get; set; }
        public Sex Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public string ImageUrl { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

    }
}
