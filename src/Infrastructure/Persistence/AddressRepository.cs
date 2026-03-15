using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LivoraDbContext _context;

        public AddressRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.Where(a => a.IsActive).ToListAsync();
        }

        public async Task<Address> CreateAsync(Address address)
        {
            address.CreatedAt = DateTime.UtcNow;
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await GetByIdAsync(id);
            if (address == null) return false;

            address.IsActive = false;
            await UpdateAsync(address);
            return true;
        }
    }
}