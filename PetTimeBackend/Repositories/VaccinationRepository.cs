using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.IRepositories;

namespace PetTimeBackend.Repositories
{
    public class VaccinationRepository : IVaccinationRepository
    {
        private readonly PetTimeContext _context;

        public VaccinationRepository(PetTimeContext context)
        {
            _context = context;
        }

        public Vaccination GetVaccinationById(long vaccinationId)
        {
            return _context.Vaccinations.Find(vaccinationId);
        }

        public IEnumerable<Vaccination> GetVaccinationsByPetId(long petId)
        {
            return _context.Vaccinations.Where(v => v.PetId == petId).ToList();
        }

        public void AddVaccination(Vaccination vaccination)
        {
            _context.Vaccinations.Add(vaccination);
        }

        public void UpdateVaccination(Vaccination vaccination)
        {
            _context.Vaccinations.Update(vaccination);
        }

        public void DeleteVaccination(long vaccinationId)
        {
            var vaccination = _context.Vaccinations.Find(vaccinationId);
            if (vaccination != null)
            {
                _context.Vaccinations.Remove(vaccination);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
