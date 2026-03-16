using Microsoft.EntityFrameworkCore;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;

namespace Livora_Lite.Infrastructure
{
    public class BillingStatusRepository : IBillingStatusRepository
    {
        private readonly LivoraDbContext _context;

        public BillingStatusRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<BillingStatus?> GetByIdAsync(int id)
        {
            return await _context.BillingStatuses.FindAsync(id);
        }

        public async Task<IEnumerable<BillingStatus>> GetAllAsync()
        {
            return await _context.BillingStatuses.ToListAsync();
        }

        public async Task<BillingStatus> CreateAsync(BillingStatus billingStatus)
        {
            _context.BillingStatuses.Add(billingStatus);
            await _context.SaveChangesAsync();
            return billingStatus;
        }

        public async Task<BillingStatus> UpdateAsync(BillingStatus billingStatus)
        {
            _context.BillingStatuses.Update(billingStatus);
            await _context.SaveChangesAsync();
            return billingStatus;
        }

        public async Task DeleteAsync(int id)
        {
            var billingStatus = await GetByIdAsync(id);
            if (billingStatus != null)
            {
                _context.BillingStatuses.Remove(billingStatus);
                await _context.SaveChangesAsync();
            }
        }
    }
}