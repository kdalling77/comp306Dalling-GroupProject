using _301247589_301276375_bright_aid_API.Models;

namespace _301247589_301276375_bright_aid_API.Services
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
        Task<Donor> GetDonorByIdAsync(long id);
        Task AddDonorAsync(Donor Donor);
        Task UpdateDonorAsync(Donor Donor);
        Task DeleteDonorAsync(long id);
        Task<bool> SaveAsync();
        Task<bool> DonorExistsAsync(long id);
    }
}
