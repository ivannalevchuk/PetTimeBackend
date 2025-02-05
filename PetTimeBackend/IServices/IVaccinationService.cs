using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IServices
{
    public interface IVaccinationService
    {
        Vaccination GetVaccinationById(long vaccinationId);
        IEnumerable<Vaccination> GetVaccinationsByPetId(long petId);
        void CreateVaccination(VaccinationViewModel model);
        void UpdateVaccination(VaccinationViewModel model);
        void DeleteVaccination(long vaccinationId);
    }
}
