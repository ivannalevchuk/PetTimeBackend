using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IRepositories
{
    public class PetRepository:IPetRepository
    {
        private readonly PetTimeContext _dbContext;

        public PetRepository(PetTimeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _dbContext.Pets.ToListAsync();
        }

        public async Task<Pet> GetPetByIdAsync(long id)
        {
            return await _dbContext.Pets.FindAsync(id);
        }
        public Pet GetPetById(long id)
        {
            return  _dbContext.Pets.Find(id);
        }

        public async Task<Pet> AddPetAsync(Pet pet)
        {
            _dbContext.Pets.Add(pet);
            await _dbContext.SaveChangesAsync();
            return pet;
        }

        public async Task UpdatePetAsync(long id, Pet updatedPet)
        {
            var existingPet = await _dbContext.Pets.FindAsync(id);
            if (existingPet != null)
            {
                existingPet.Name = updatedPet.Name;
                
                await _dbContext.SaveChangesAsync();
            }
        }
        public  IEnumerable<Pet> GetPetByUserId(long userId)
        {
            return _dbContext.Pets.Where(p => p.UserId == userId).ToList();
        }
        public async Task DeletePetAsync(long id)
        {
            var pet = await _dbContext.Pets.FindAsync(id);
            if (pet != null)
            {
                _dbContext.Pets.Remove(pet);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
