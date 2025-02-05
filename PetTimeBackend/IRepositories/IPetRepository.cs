using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IRepositories
{
    public interface IPetRepository
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        IEnumerable<Pet> GetPetByUserId(long userId);
        Task<Pet> GetPetByIdAsync(long id);
        Task<Pet> AddPetAsync(Pet pet);
        Task UpdatePetAsync(long id, Pet updatedPet);
        Task DeletePetAsync(long id);
        Pet GetPetById(long id);
    }
}
