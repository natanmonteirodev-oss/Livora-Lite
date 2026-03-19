using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class BillingStatusService : IBillingStatusService
    {
        private readonly IBillingStatusRepository _billingStatusRepository;
        private readonly IMapper _mapper;

        public BillingStatusService(IBillingStatusRepository billingStatusRepository, IMapper mapper)
        {
            _billingStatusRepository = billingStatusRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillingStatusDTO>> GetAllBillingStatusesAsync()
        {
            var billingStatuses = await _billingStatusRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BillingStatusDTO>>(billingStatuses);
        }

        public async Task<BillingStatusDTO?> GetBillingStatusByIdAsync(int id)
        {
            var billingStatus = await _billingStatusRepository.GetByIdAsync(id);
            return billingStatus == null ? null : _mapper.Map<BillingStatusDTO>(billingStatus);
        }
    }
}