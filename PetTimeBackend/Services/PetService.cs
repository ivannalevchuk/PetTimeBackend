using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetTimeBackend.Entities;
using PetTimeBackend.IRepositories;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PetTimeBackend.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _petRepository.GetAllPetsAsync();
        }

        public async Task<Pet> GetPetByIdAsync(long id)
        {
            return await _petRepository.GetPetByIdAsync(id);
        }

        public async Task<Pet> AddPetAsync(PetViewModel model)
        {
            var pet = new Pet
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                BreedId = model.BreedId,
                Sex = model.Sex,
                UserId = model.UserId,
                Weight = model.Weight,

            };

            return await _petRepository.AddPetAsync(pet);
        }

        public async Task UpdatePetAsync(long id, PetViewModel model)
        {
            var updatedPet = new Pet
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                BreedId = model.BreedId,
                Sex = model.Sex,
                UserId = model.UserId

            };
            await _petRepository.UpdatePetAsync(id, updatedPet);
        }

        public async Task DeletePetAsync(long id)
        {
            await _petRepository.DeletePetAsync(id);
        }
    }
}
