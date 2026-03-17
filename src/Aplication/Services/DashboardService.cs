using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IBillingRepository _billingRepository;

        public DashboardService(
            IUserRepository userRepository,
            IPropertyRepository propertyRepository,
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IPaymentRepository paymentRepository,
            IMaintenanceRequestRepository maintenanceRequestRepository,
            IAuditLogRepository auditLogRepository,
            IBillingRepository billingRepository)
        {
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _paymentRepository = paymentRepository;
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _auditLogRepository = auditLogRepository;
            _billingRepository = billingRepository;
        }

        public async Task<AdminDashboardDTO> GetAdminDashboardAsync()
        {
            try
            {
                var users = (await _userRepository.GetAllAsync()).ToList();
                var properties = (await _propertyRepository.GetAllAsync()).ToList();
                var tenants = (await _tenantRepository.GetAllAsync()).ToList();
                var contracts = (await _contractRepository.GetAllAsync()).ToList();
                var billings = (await _billingRepository.GetAllAsync()).ToList();
                var maintenance = (await _maintenanceRequestRepository.GetAllAsync()).ToList();
                var auditLogs = (await _auditLogRepository.GetAllAuditLogsAsync()).ToList();

                var now = DateTime.UtcNow;
                var activeContracts = contracts.Where(c => c.StartDate <= now && (c.EndDate == null || c.EndDate > now)).ToList();
                var expiredContracts = contracts.Where(c => c.EndDate != null && c.EndDate < now).ToList();

                var pendingBillings = billings.Where(b => b.BillingStatus?.Name == "Pending" || b.BillingStatus?.Name == "Overdue").ToList();
                var pendingMaintenance = maintenance.Where(m => m.Status == MaintenanceStatus.Aberta || m.Status == MaintenanceStatus.EmAndamento).ToList();

                return new AdminDashboardDTO
                {
                    TotalUsers = users.Count(u => u.IsActive),
                    TotalProperties = properties.Count(p => p.IsActive),
                    TotalTenants = tenants.Count(t => t.IsActive),
                    TotalContracts = contracts.Count(),
                    TotalContracts_Active = activeContracts.Count,
                    TotalContracts_Expired = expiredContracts.Count,
                    TotalRevenueExpected = activeContracts.Sum(c => c.RentValue),
                    PendingPayments = pendingBillings.Count,
                    PendingPaymentsAmount = pendingBillings.Sum(p => p.Amount),
                    PendingMaintenance = pendingMaintenance.Count,
                    RecentActivities = auditLogs
                        .OrderByDescending(a => a.Date)
                        .Take(10)
                        .Select(a => new RecentActivityDTO
                        {
                            Id = a.Id,
                            Action = a.Action,
                            Entity = a.Entity,
                            UserName = a.UserName,
                            CreatedAt = a.Date,
                            Changes = a.Changes
                        }).ToList(),
                    TopProperties = properties
                        .Take(5)
                        .Select(p => new PropertySummaryDTO
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Address = p.Address?.Street ?? "N/A",
                            Status = p.PropertyStatus?.Name ?? "N/A",
                            TotalMonthlyRevenue = activeContracts.Where(c => c.PropertyId == p.Id).Sum(c => c.RentValue),
                            ActiveTenants = activeContracts.Count(c => c.PropertyId == p.Id)
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                // Log error
                return new AdminDashboardDTO();
            }
        }

        public async Task<OwnerDashboardDTO> GetOwnerDashboardAsync(int userId)
        {
            try
            {
                // Buscar apenas propriedades do proprietário
                var properties = (await _propertyRepository.GetByOwnerIdAsync(userId)).ToList();
                var contracts = (await _contractRepository.GetAllAsync()).ToList();
                var tenants = (await _tenantRepository.GetAllAsync()).ToList();
                var billings = (await _billingRepository.GetAllAsync()).ToList();
                var maintenance = (await _maintenanceRequestRepository.GetAllAsync()).ToList();

                // Filtrar contratos apenas das propriedades do proprietário
                var propertyIds = properties.Select(p => p.Id).ToList();
                var ownerContracts = contracts.Where(c => propertyIds.Contains(c.PropertyId)).ToList();
                var ownerBillings = billings.Where(b => propertyIds.Contains((contracts.FirstOrDefault(c => c.Id == b.ContractId)?.PropertyId ?? 0))).ToList();

                var now = DateTime.UtcNow;
                var activeContracts = ownerContracts.Where(c => c.StartDate <= now && (c.EndDate == null || c.EndDate > now)).ToList();
                var expiringContracts = activeContracts.Where(c => c.EndDate != null && c.EndDate < now.AddDays(30)).ToList();

                var pendingBillings = ownerBillings.Where(b => b.BillingStatus?.Name == "Pending" || b.BillingStatus?.Name == "Overdue").ToList();

                return new OwnerDashboardDTO
                {
                    TotalProperties = properties.Count(p => p.IsActive),
                    ActiveContracts = activeContracts.Count,
                    ExpiringContracts = expiringContracts.Count,
                    TotalMonthlyRevenue = activeContracts.Sum(c => c.RentValue),
                    PendingPayments = pendingBillings.Count,
                    PendingPaymentsAmount = pendingBillings.Sum(p => p.Amount),
                    MaintenanceRequests = maintenance.Count(m => m.Status == MaintenanceStatus.Aberta || m.Status == MaintenanceStatus.EmAndamento),
                    Properties = properties.Select(p => new PropertyDetailDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Address = p.Address?.Street ?? "N/A",
                        PropertyType = p.PropertyType?.Name ?? "N/A",
                        Status = p.PropertyStatus?.Name ?? "N/A",
                        Area = p.Area,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        SuggestedRentValue = p.SuggestedRentValue,
                        ActiveContracts = activeContracts.Count(c => c.PropertyId == p.Id),
                        TotalMonthlyRevenue = activeContracts.Where(c => c.PropertyId == p.Id).Sum(c => c.RentValue)
                    }).ToList(),
                    ActiveContracts_Details = activeContracts.Select(c => new ContractSummaryDTO
                    {
                        Id = c.Id,
                        PropertyName = properties.FirstOrDefault(p => p.Id == c.PropertyId)?.Name ?? "N/A",
                        PropertyAddress = properties.FirstOrDefault(p => p.Id == c.PropertyId)?.Address?.Street ?? "N/A",
                        TenantName = (tenants.FirstOrDefault(t => t.Id == c.TenantId) != null ? $"{tenants.FirstOrDefault(t => t.Id == c.TenantId)?.FirstName} {tenants.FirstOrDefault(t => t.Id == c.TenantId)?.LastName}" : "N/A"),
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        RentValue = c.RentValue,
                        DueDay = c.DueDay,
                        IsExpiringSoon = c.EndDate != null && c.EndDate < now.AddDays(30),
                        Status = c.EndDate != null && c.EndDate < now ? "Expired" : "Active"
                    }).ToList(),
                    PendingPayments_Details = pendingBillings.Select(p => new PaymentPendingDTO
                    {
                        Id = p.Id,
                        PropertyName = properties.FirstOrDefault(pr => pr.Id == (contracts.FirstOrDefault(c => c.Id == p.ContractId)?.PropertyId ?? 0))?.Name ?? "N/A",
                        TenantName = (contracts.FirstOrDefault(c => c.Id == p.ContractId)?.TenantId != null && tenants.FirstOrDefault(t => t.Id == contracts.FirstOrDefault(c => c.Id == p.ContractId)?.TenantId) != null ? $"{tenants.FirstOrDefault(t => t.Id == contracts.FirstOrDefault(c => c.Id == p.ContractId)?.TenantId)?.FirstName} {tenants.FirstOrDefault(t => t.Id == contracts.FirstOrDefault(c => c.Id == p.ContractId)?.TenantId)?.LastName}" : "N/A"),
                        Amount = p.Amount,
                        DueDate = p.DueDate,
                        DaysOverdue = (int)(DateTime.UtcNow - p.DueDate).TotalDays,
                        Status = p.BillingStatus?.Name ?? "Pending"
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                // Log error
                return new OwnerDashboardDTO();
            }
        }

        public async Task<TenantDashboardDTO> GetTenantDashboardAsync(int userId)
        {
            try
            {
                // Buscar o tenant associado ao usuário
                var tenant = await _tenantRepository.GetByUserIdAsync(userId);
                if (tenant == null)
                {
                    return new TenantDashboardDTO(); // Usuário não é um inquilino
                }

                var contracts = (await _contractRepository.GetAllAsync()).ToList();
                var properties = (await _propertyRepository.GetAllAsync()).ToList();
                var billings = (await _billingRepository.GetAllAsync()).ToList();
                var payments = (await _paymentRepository.GetAllAsync()).ToList();
                var maintenance = (await _maintenanceRequestRepository.GetAllAsync()).ToList();

                // Filtrar apenas contratos do tenant
                var tenantContracts = contracts.Where(c => c.TenantId == tenant.Id).ToList();
                var tenantBillings = billings.Where(b => tenantContracts.Any(c => c.Id == b.ContractId)).ToList();
                var tenantMaintenance = maintenance.Where(m => tenantContracts.Any(c => c.Id == m.ContractId)).ToList();
                var tenantPayments = payments.Where(p => tenantBillings.Any(b => b.Id == p.BillingId)).ToList();

                var now = DateTime.UtcNow;
                var activeContracts = tenantContracts.Where(c => c.StartDate <= now && (c.EndDate == null || c.EndDate > now)).ToList();
                var expiringContracts = activeContracts.Where(c => c.EndDate != null && c.EndDate < now.AddDays(30)).ToList();

                var pendingBillings = tenantBillings.Where(b => b.BillingStatus?.Name == "Pending" || b.BillingStatus?.Name == "Overdue").ToList();
                var nextPaymentDue = tenantBillings.Where(b => b.DueDate > now).OrderBy(b => b.DueDate).FirstOrDefault()?.DueDate ?? DateTime.UtcNow.AddMonths(1);

                return new TenantDashboardDTO
                {
                    ActiveContracts = activeContracts.Count,
                    ExpiringContracts = expiringContracts.Count,
                    TotalMonthlyRent = activeContracts.Sum(c => c.RentValue),
                    PendingPayments = pendingBillings.Count,
                    PendingPaymentsAmount = pendingBillings.Sum(p => p.Amount),
                    NextPaymentDue = nextPaymentDue,
                    MaintenanceRequests = tenantMaintenance.Count(),
                    ActiveContracts_Details = activeContracts.Select(c => new ContractSummaryDTO
                    {
                        Id = c.Id,
                        PropertyName = properties.FirstOrDefault(p => p.Id == c.PropertyId)?.Name ?? "N/A",
                        PropertyAddress = properties.FirstOrDefault(p => p.Id == c.PropertyId)?.Address?.Street ?? "N/A",
                        TenantName = $"{tenant.FirstName} {tenant.LastName}",
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        RentValue = c.RentValue,
                        DueDay = c.DueDay,
                        IsExpiringSoon = c.EndDate != null && c.EndDate < now.AddDays(30),
                        Status = c.EndDate != null && c.EndDate < now ? "Expired" : "Active"
                    }).ToList(),
                    RecentPayments = tenantPayments
                        .OrderByDescending(p => p.PaymentDate)
                        .Take(10)
                        .Select(p => new PaymentHistoryDTO
                        {
                            Id = p.Id,
                            PropertyName = properties.FirstOrDefault(pr => pr.Id == (p.Billing?.Contract?.PropertyId ?? 0))?.Name ?? "N/A",
                            Amount = p.AmountPaid,
                            PaymentDate = p.PaymentDate,
                            PaymentMethod = p.PaymentMethod?.Name ?? "N/A",
                            Status = p.IsActive ? "Completed" : "Cancelled"
                        }).ToList(),
                    MaintenanceRequests_Details = tenantMaintenance
                        .OrderByDescending(m => m.RequestDate)
                        .Take(5)
                        .Select(m => new MaintenanceRequestSummaryDTO
                        {
                            Id = m.Id,
                            Title = m.Description,
                            PropertyName = properties.FirstOrDefault(p => p.Id == (contracts.FirstOrDefault(c => c.Id == m.ContractId)?.PropertyId ?? 0))?.Name ?? "N/A",
                            Description = m.Description,
                            RequestedDate = m.CreatedAt,
                            Status = m.Status.ToString(),
                            Priority = m.Priority.ToString()
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                // Log error
                return new TenantDashboardDTO();
            }
        }
    }
}
