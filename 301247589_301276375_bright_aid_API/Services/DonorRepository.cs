using _301247589_301276375_bright_aid_API.Models;
using Microsoft.EntityFrameworkCore;

namespace _301247589_301276375_bright_aid_API.Services
{
    public class DonorRepository : IDonorRepository
    {
        private readonly DonorContext _context;

        public DonorRepository(DonorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donor>> GetAllDonorsAsync()
        {
            return await _context.Donors.Where(d => d.IsActive).ToListAsync();
        }

        public async Task<Donor> GetDonorByIdAsync(long id)
        {
            return await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == id && d.IsActive);
        }

        public async Task AddDonorAsync(Donor Donor)
        {
            await _context.Donors.AddAsync(Donor);
        }

        public async Task UpdateDonorAsync(Donor Donor)
        {
            _context.Entry(Donor).State = EntityState.Modified;
        }

        public async Task DeleteDonorAsync(long id)
        {
            var Donor = await GetDonorByIdAsync(id);
            if (Donor != null)
            {
                Donor.IsActive = false; // Mark as inactive
                _context.Entry(Donor).State = EntityState.Modified;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DonorExistsAsync(long id)
        {
            return await _context.Donors.AnyAsync(e => e.DonorId == id);
        }
    }
}
