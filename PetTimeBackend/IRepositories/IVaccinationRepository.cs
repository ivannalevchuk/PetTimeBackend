using PetTimeBackend.Entities;

namespace PetTimeBackend.IRepositories
{
    public interface IVaccinationRepository
    {
        Vaccination GetVaccinationById(long vaccinationId);
        IEnumerable<Vaccination> GetVaccinationsByPetId(long petId);

        void AddVaccination(Vaccination vaccination);
        void UpdateVaccination(Vaccination vaccination);
        void DeleteVaccination(long vaccinationId);
        void Save();
    }
}
