namespace Livora_Lite.Application.DTO
{
    // Financial Report DTO
    public class FinancialReportDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal CollectionRate { get; set; }
        public List<MonthlyRevenueDTO> MonthlyRevenue { get; set; } = new();
        public List<PaymentMethodReportDTO> PaymentMethods { get; set; } = new();
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
    }

    // Monthly Revenue DTO
    public class MonthlyRevenueDTO
    {
        public string Month { get; set; } = string.Empty;
        public decimal ExpectedRevenue { get; set; }
        public decimal ReceivedRevenue { get; set; }
        public decimal Difference { get; set; }
    }

    // Payment Method Report DTO
    public class PaymentMethodReportDTO
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
    }

    // Property Performance Report DTO
    public class PropertyPerformanceReportDTO
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int ActiveTenants { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal OccupancyRate { get; set; }
        public int MaintenanceRequests { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal NetProfit { get; set; }
        public string PerformanceRating { get; set; } = string.Empty;
    }

    // Contract Analysis Report DTO
    public class ContractAnalysisReportDTO
    {
        public int TotalContracts { get; set; }
        public int ActiveContracts { get; set; }
        public int ExpiringContracts { get; set; }
        public int ExpiredContracts { get; set; }
        public decimal AverageRentValue { get; set; }
        public decimal TotalMonthlyRevenue { get; set; }
        public List<ContractDistributionDTO> DistributionByStatus { get; set; } = new();
        public List<ContractExpiringDTO> ExpiringContractsList { get; set; } = new();
    }

    // Contract Distribution DTO
    public class ContractDistributionDTO
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    // Expiring Contract DTO
    public class ContractExpiringDTO
    {
        public int ContractId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public DateTime EndDate { get; set; }
        public int DaysUntilExpiration { get; set; }
        public decimal RentValue { get; set; }
    }

    // Maintenance Report DTO
    public class MaintenanceReportDTO
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
        public decimal AverageCostPerRequest { get; set; }
        public decimal TotalCost { get; set; }
        public List<MaintenanceByPriorityDTO> ByPriority { get; set; } = new();
        public List<MaintenanceByPropertyDTO> ByProperty { get; set; } = new();
        public List<RecentMaintenanceDTO> RecentRequests { get; set; } = new();
    }

    // Maintenance by Priority DTO
    public class MaintenanceByPriorityDTO
    {
        public string Priority { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    // Maintenance by Property DTO
    public class MaintenanceByPropertyDTO
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
        public decimal TotalCost { get; set; }
    }

    // Recent Maintenance DTO
    public class RecentMaintenanceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
    }
}
