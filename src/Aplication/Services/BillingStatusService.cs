using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class BillingStatusService : IBillingStatusService
    {
        private readonly IBillingStatusRepository _billingStatusRepository;

        public BillingStatusService(IBillingStatusRepository billingStatusRepository)
        {
            _billingStatusRepository = billingStatusRepository;
        }

        public async Task<IEnumerable<BillingStatusDTO>> GetAllBillingStatusesAsync()
        {
            var billingStatuses = await _billingStatusRepository.GetAllAsync();
            return billingStatuses.Select(bs => new BillingStatusDTO
            {
                Id = bs.Id,
                Name = bs.Name,
                CreatedAt = bs.CreatedAt
            });
        }

        public async Task<BillingStatusDTO?> GetBillingStatusByIdAsync(int id)
        {
            var billingStatus = await _billingStatusRepository.GetByIdAsync(id);
            if (billingStatus == null) return null;

            return new BillingStatusDTO
            {
                Id = billingStatus.Id,
                Name = billingStatus.Name,
                CreatedAt = billingStatus.CreatedAt
            };
        }
    }
}