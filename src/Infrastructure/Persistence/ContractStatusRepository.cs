using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class ContractStatusRepository : IContractStatusRepository
    {
        private readonly LivoraDbContext _context;

        public ContractStatusRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<ContractStatus?> GetByIdAsync(int id)
        {
            return await _context.ContractStatuses.FindAsync(id);
        }

        public async Task<IEnumerable<ContractStatus>> GetAllAsync()
        {
            return await _context.ContractStatuses.ToListAsync();
        }

        public async Task<ContractStatus> CreateAsync(ContractStatus contractStatus)
        {
            _context.ContractStatuses.Add(contractStatus);
            await _context.SaveChangesAsync();
            return contractStatus;
        }

        public async Task<ContractStatus> UpdateAsync(ContractStatus contractStatus)
        {
            _context.ContractStatuses.Update(contractStatus);
            await _context.SaveChangesAsync();
            return contractStatus;
        }

        public async Task DeleteAsync(int id)
        {
            var contractStatus = await GetByIdAsync(id);
            if (contractStatus != null)
            {
                _context.ContractStatuses.Remove(contractStatus);
                await _context.SaveChangesAsync();
            }
        }
    }
}