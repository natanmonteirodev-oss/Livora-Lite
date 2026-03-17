namespace Livora_Lite.Application.DTO
{
    // Dashboard para Admin
    public class AdminDashboardDTO
    {
        public int TotalUsers { get; set; }
        public int TotalProperties { get; set; }
        public int TotalTenants { get; set; }
        public int TotalContracts { get; set; }
        public int TotalContracts_Active { get; set; }
        public int TotalContracts_Expired { get; set; }
        public decimal TotalRevenueExpected { get; set; }
        public int PendingPayments { get; set; }
        public decimal PendingPaymentsAmount { get; set; }
        public int PendingMaintenance { get; set; }
        public List<RecentActivityDTO> RecentActivities { get; set; } = new();
        public List<PropertySummaryDTO> TopProperties { get; set; } = new();
    }

    // Dashboard para Proprietário (Owner)
    public class OwnerDashboardDTO
    {
        public int TotalProperties { get; set; }
        public int ActiveContracts { get; set; }
        public int ExpiringContracts { get; set; }
        public decimal TotalMonthlyRevenue { get; set; }
        public int PendingPayments { get; set; }
        public decimal PendingPaymentsAmount { get; set; }
        public int MaintenanceRequests { get; set; }
        public List<PropertyDetailDTO> Properties { get; set; } = new();
        public List<ContractSummaryDTO> ActiveContracts_Details { get; set; } = new();
        public List<PaymentPendingDTO> PendingPayments_Details { get; set; } = new();
    }

    // Dashboard para Inquilino (Tenant)
    public class TenantDashboardDTO
    {
        public int ActiveContracts { get; set; }
        public int ExpiringContracts { get; set; }
        public decimal TotalMonthlyRent { get; set; }
        public int PendingPayments { get; set; }
        public decimal PendingPaymentsAmount { get; set; }
        public DateTime NextPaymentDue { get; set; }
        public int MaintenanceRequests { get; set; }
        public List<ContractSummaryDTO> ActiveContracts_Details { get; set; } = new();
        public List<PaymentHistoryDTO> RecentPayments { get; set; } = new();
        public List<MaintenanceRequestSummaryDTO> MaintenanceRequests_Details { get; set; } = new();
    }

    // DTOs auxiliares
    public class PropertyDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal SuggestedRentValue { get; set; }
        public int ActiveContracts { get; set; }
        public decimal TotalMonthlyRevenue { get; set; }
    }

    public class PropertySummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalMonthlyRevenue { get; set; }
        public int ActiveTenants { get; set; }
    }

    public class ContractSummaryDTO
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentValue { get; set; }
        public int DueDay { get; set; }
        public bool IsExpiringSoon { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PaymentPendingDTO
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PaymentHistoryDTO
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class MaintenanceRequestSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public class RecentActivityDTO
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Changes { get; set; } = string.Empty;
    }
}
