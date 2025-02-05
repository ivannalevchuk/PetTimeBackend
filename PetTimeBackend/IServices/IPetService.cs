using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IServices
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<Pet> GetPetByIdAsync(long id);
       
        Task<Pet> AddPetAsync(PetViewModel pet);
        Task UpdatePetAsync(long id, PetViewModel updatedPet);
        Task DeletePetAsync(long id);
    }
}
