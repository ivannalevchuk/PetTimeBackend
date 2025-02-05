using PetTimeBackend.Entities;
using PetTimeBackend.IRepositories;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly IVaccinationRepository _vaccinationRepository;

        public VaccinationService(IVaccinationRepository vaccinationRepository)
        {
            _vaccinationRepository = vaccinationRepository;
        }

        public Vaccination GetVaccinationById(long vaccinationId)
        {
            return _vaccinationRepository.GetVaccinationById(vaccinationId);
        }

        public IEnumerable<Vaccination> GetVaccinationsByPetId(long petId)
        {
            return _vaccinationRepository.GetVaccinationsByPetId(petId);
        }

        public void CreateVaccination(VaccinationViewModel model)
        {
            var vaccination = new Vaccination
            {
                Name = model.Name,
                Date = model.Date,
                PetId = model.PetId
            };

            _vaccinationRepository.AddVaccination(vaccination);
            _vaccinationRepository.Save();
        }

        public void UpdateVaccination(VaccinationViewModel model)
        {
            var vaccination = _vaccinationRepository.GetVaccinationById(model.Id);
            if (vaccination == null)
            {
                throw new Exception("Vaccination not found");
            }

            vaccination.Name = model.Name;
            vaccination.Date = model.Date;
            vaccination.PetId = model.PetId;

            _vaccinationRepository.UpdateVaccination(vaccination);
            _vaccinationRepository.Save();
        }

        public void DeleteVaccination(long vaccinationId)
        {
            _vaccinationRepository.DeleteVaccination(vaccinationId);
            _vaccinationRepository.Save();
        }
    }
}
