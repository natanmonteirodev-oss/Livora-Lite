using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IReportService
    {
        Task<FinancialReportDTO> GetFinancialReportAsync();
        Task<List<PropertyPerformanceReportDTO>> GetPropertyPerformanceReportAsync();
        Task<ContractAnalysisReportDTO> GetContractAnalysisReportAsync();
        Task<MaintenanceReportDTO> GetMaintenanceReportAsync();
    }
}
