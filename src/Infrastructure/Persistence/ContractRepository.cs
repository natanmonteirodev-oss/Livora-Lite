#nullable disable

using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class ContractRepository : IContractRepository
    {
        private readonly LivoraDbContext _context;

        public ContractRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Tenant)
                .Include(c => c.ContractStatus)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Contract>> GetAllAsync()
        {
            return await _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Tenant)
                .Include(c => c.ContractStatus)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Contract> CreateAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract> UpdateAsync(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task DeleteAsync(int id)
        {
            var contract = await GetByIdAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetActiveContractsAsync()
        {
            return await _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Tenant)
                .Include(c => c.ContractStatus)
                .Where(c => c.IsActive && c.ContractStatus.Name == "Ativo")
                .ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetContractsByPropertyAsync(int propertyId)
        {
            return await _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Tenant)
                .Include(c => c.ContractStatus)
                .Where(c => c.PropertyId == propertyId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetContractsByTenantAsync(int tenantId)
        {
            return await _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Tenant)
                .Include(c => c.ContractStatus)
                .Where(c => c.TenantId == tenantId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasActiveContractForPropertyAsync(int propertyId)
        {
            return await _context.Contracts
                .AnyAsync(c => c.PropertyId == propertyId &&
                              c.IsActive &&
                              c.ContractStatus.Name == "Ativo");
        }
    }
}