#nullable disable

using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class BillingRepository : IBillingRepository
    {
        private readonly LivoraDbContext _context;

        public BillingRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Billing?> GetByIdAsync(int id)
        {
            return await _context.Billings
                .Include(b => b.Contract)
                    .ThenInclude(c => c.Property)
                .Include(b => b.Contract)
                    .ThenInclude(c => c.Tenant)
                .Include(b => b.BillingStatus)
                .Include(b => b.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Billing>> GetAllAsync()
        {
            return await _context.Billings
                .Include(b => b.Contract)
                    .ThenInclude(c => c.Property)
                .Include(b => b.Contract)
                    .ThenInclude(c => c.Tenant)
                .Include(b => b.BillingStatus)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Billing> CreateAsync(Billing billing)
        {
            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();
            return billing;
        }

        public async Task<Billing> UpdateAsync(Billing billing)
        {
            _context.Billings.Update(billing);
            await _context.SaveChangesAsync();
            return billing;
        }

        public async Task DeleteAsync(int id)
        {
            var billing = await GetByIdAsync(id);
            if (billing != null)
            {
                _context.Billings.Remove(billing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Billing>> GetBillingsByContractAsync(int contractId)
        {
            return await _context.Billings
                .Include(b => b.Contract)
                .Include(b => b.BillingStatus)
                .Include(b => b.Payments)
                .Where(b => b.ContractId == contractId)
                .OrderByDescending(b => b.Period)
                .ToListAsync();
        }

        public async Task<IEnumerable<Billing>> GetBillingsByPeriodAsync(string period)
        {
            return await _context.Billings
                .Include(b => b.Contract)
                .Include(b => b.BillingStatus)
                .Where(b => b.Period == period)
                .OrderBy(b => b.ContractId)
                .ToListAsync();
        }

        public async Task<bool> HasBillingForContractAndPeriodAsync(int contractId, string period)
        {
            return await _context.Billings
                .AnyAsync(b => b.ContractId == contractId && b.Period == period && b.IsActive);
        }
    }
}