namespace PetTimeBackend.ViewModels
{
    public class VaccinationViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public long PetId { get; set; }
    }
}
