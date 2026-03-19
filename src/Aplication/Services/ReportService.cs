using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMaintenanceRequestRepository _maintenanceRepository;
        private readonly IBillingRepository _billingRepository;

        public ReportService(
            IPaymentRepository paymentRepository,
            IPropertyRepository propertyRepository,
            IContractRepository contractRepository,
            IMaintenanceRequestRepository maintenanceRepository,
            IBillingRepository billingRepository)
        {
            _paymentRepository = paymentRepository;
            _propertyRepository = propertyRepository;
            _contractRepository = contractRepository;
            _maintenanceRepository = maintenanceRepository;
            _billingRepository = billingRepository;
        }

        public async Task<FinancialReportDTO> GetFinancialReportAsync()
        {
            var report = new FinancialReportDTO
            {
                GeneratedDate = DateTime.Now,
                TotalRevenue = 0,
                TotalExpenses = 0,
                PendingPayments = 0,
                PaymentMethods = new List<PaymentMethodReportDTO>(),
                MonthlyRevenue = GenerateMonthlyRevenue()
            };

            // Get payments data
            try
            {
                var payments = await _paymentRepository.GetAllAsync();
                var billings = await _billingRepository.GetAllAsync();

                report.TotalRevenue = payments.Sum(p => p.AmountPaid);
                report.PendingPayments = billings.Where(b => b.BillingStatus?.Name == "Pendente").Sum(b => b.Amount);
                
                if (payments.Any())
                {
                    report.CollectionRate = (report.TotalRevenue / (report.TotalRevenue + report.PendingPayments)) * 100;
                }

                // Group by payment method
                var methodGroups = payments.GroupBy(p => p.PaymentMethod?.Name ?? "Não especificado");
                var totalAmount = payments.Sum(p => p.AmountPaid);

                foreach (var group in methodGroups)
                {
                    var amount = group.Sum(p => p.AmountPaid);
                    report.PaymentMethods.Add(new PaymentMethodReportDTO
                    {
                        PaymentMethod = group.Key,
                        Count = group.Count(),
                        TotalAmount = amount,
                        Percentage = totalAmount > 0 ? (amount / totalAmount) * 100 : 0
                    });
                }

                report.NetProfit = report.TotalRevenue - report.TotalExpenses;
            }
            catch { }

            return report;
        }

        public async Task<List<PropertyPerformanceReportDTO>> GetPropertyPerformanceReportAsync()
        {
            var report = new List<PropertyPerformanceReportDTO>();

            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var allContracts = await _contractRepository.GetAllAsync();
                var allMaintenance = await _maintenanceRepository.GetAllAsync();

                foreach (var property in properties)
                {
                    var contracts = allContracts.Where(c => c.PropertyId == property.Id).ToList();
                    var activeContracts = contracts.Where(c => c.EndDate >= DateTime.Now || c.EndDate == null).ToList();
                    var maintenances = allMaintenance.Where(m => m.PropertyId == property.Id).ToList();

                    var monthlyRevenue = activeContracts.Sum(c => c.RentValue);
                    var netProfit = monthlyRevenue;
                    var occupancyRate = contracts.Any() ? (activeContracts.Count / (decimal)contracts.Count) * 100 : 0;

                    var performanceRating = CalculatePerformanceRating(occupancyRate, netProfit);

                    var address = property.Address;
                    var addressText = address != null 
                        ? $"{address.Street}, {address.Number} - {address.Neighborhood}, {address.City}-{address.State}"
                        : "Não informado";

                    report.Add(new PropertyPerformanceReportDTO
                    {
                        PropertyId = property.Id,
                        PropertyName = property.Name,
                        Address = addressText,
                        ActiveTenants = activeContracts.Count(),
                        MonthlyRevenue = monthlyRevenue,
                        OccupancyRate = occupancyRate,
                        MaintenanceRequests = maintenances.Count(),
                        MaintenanceCost = 0,
                        NetProfit = netProfit,
                        PerformanceRating = performanceRating
                    });
                }
            }
            catch { }

            return report;
        }

        public async Task<ContractAnalysisReportDTO> GetContractAnalysisReportAsync()
        {
            var report = new ContractAnalysisReportDTO();

            try
            {
                var contracts = await _contractRepository.GetAllAsync();

                report.TotalContracts = contracts.Count();
                report.ActiveContracts = contracts.Count(c => c.EndDate >= DateTime.Now || c.EndDate == null);
                report.ExpiringContracts = contracts.Count(c => c.EndDate > DateTime.Now && c.EndDate <= DateTime.Now.AddDays(30));
                report.ExpiredContracts = contracts.Count(c => c.EndDate < DateTime.Now);

                report.AverageRentValue = contracts.Any() ? contracts.Average(c => c.RentValue) : 0;
                report.TotalMonthlyRevenue = contracts.Where(c => c.EndDate >= DateTime.Now || c.EndDate == null).Sum(c => c.RentValue);

                // Distribution by status
                var statuses = new[] { "Active", "Expiring", "Expired" };
                foreach (var status in statuses)
                {
                    int count = status == "Active" ? report.ActiveContracts
                        : status == "Expiring" ? report.ExpiringContracts
                        : report.ExpiredContracts;

                    if (report.TotalContracts > 0)
                    {
                        report.DistributionByStatus.Add(new ContractDistributionDTO
                        {
                            Status = status switch
                            {
                                "Active" => "Ativo",
                                "Expiring" => "Expirando",
                                "Expired" => "Expirado",
                                _ => status
                            },
                            Count = count,
                            Percentage = (count / (decimal)report.TotalContracts) * 100
                        });
                    }
                }

                // Expiring contracts list
                report.ExpiringContractsList = contracts
                    .Where(c => c.EndDate > DateTime.Now && c.EndDate <= DateTime.Now.AddDays(30))
                    .OrderBy(c => c.EndDate)
                    .Select(c => new ContractExpiringDTO
                    {
                        ContractId = c.Id,
                        PropertyName = c.Property?.Name ?? "Não informado",
                        TenantName = c.Tenant?.FirstName + " " + c.Tenant?.LastName ?? "Não informado",
                        EndDate = c.EndDate ?? DateTime.Now,
                        DaysUntilExpiration = c.EndDate.HasValue ? (int)(c.EndDate.Value - DateTime.Now).TotalDays : 0,
                        RentValue = c.RentValue
                    })
                    .ToList();
            }
            catch { }

            return report;
        }

        public async Task<MaintenanceReportDTO> GetMaintenanceReportAsync()
        {
            var report = new MaintenanceReportDTO();

            try
            {
                var maintenances = await _maintenanceRepository.GetAllAsync();

                report.TotalRequests = maintenances.Count();
                report.PendingRequests = maintenances.Count(m => m.Status == MaintenanceStatus.Aberta);
                report.InProgressRequests = maintenances.Count(m => m.Status == MaintenanceStatus.EmAndamento);
                report.CompletedRequests = maintenances.Count(m => m.Status == MaintenanceStatus.Concluida);

                report.TotalCost = 0; // MaintenanceRequest não tem campo Cost
                report.AverageCostPerRequest = 0;

                // By Priority
                var priorities = new[] { MaintenancePriority.Baixa, MaintenancePriority.Media, MaintenancePriority.Alta, MaintenancePriority.Urgente };
                foreach (var priority in priorities)
                {
                    var count = maintenances.Count(m => m.Priority == priority);
                    if (report.TotalRequests > 0)
                    {
                        report.ByPriority.Add(new MaintenanceByPriorityDTO
                        {
                            Priority = priority switch
                            {
                                MaintenancePriority.Baixa => "Baixa",
                                MaintenancePriority.Media => "Média",
                                MaintenancePriority.Alta => "Alta",
                                MaintenancePriority.Urgente => "Urgente",
                                _ => priority.ToString()
                            },
                            Count = count,
                            Percentage = (count / (decimal)report.TotalRequests) * 100
                        });
                    }
                }

                // By Property
                var maintenancesByProperty = maintenances.GroupBy(m => m.Property?.Name ?? "Não informado");
                foreach (var group in maintenancesByProperty)
                {
                    report.ByProperty.Add(new MaintenanceByPropertyDTO
                    {
                        PropertyId = group.First().PropertyId,
                        PropertyName = group.Key,
                        RequestCount = group.Count(),
                        TotalCost = 0
                    });
                }

                // Recent Requests
                report.RecentRequests = maintenances
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(10)
                    .Select(m => new RecentMaintenanceDTO
                    {
                        Id = m.Id,
                        Title = m.Title,
                        PropertyName = m.Property?.Name ?? "Não informado",
                        Priority = m.Priority switch
                        {
                            MaintenancePriority.Baixa => "Baixa",
                            MaintenancePriority.Media => "Média",
                            MaintenancePriority.Alta => "Alta",
                            MaintenancePriority.Urgente => "Urgente",
                            _ => m.Priority.ToString()
                        },
                        Status = m.Status switch
                        {
                            MaintenanceStatus.Aberta => "Aberta",
                            MaintenanceStatus.EmAndamento => "Em Andamento",
                            MaintenanceStatus.AguardandoOrcamento => "Aguardando Orçamento",
                            MaintenanceStatus.Concluida => "Concluída",
                            _ => m.Status.ToString()
                        },
                        RequestedDate = m.CreatedAt
                    })
                    .ToList();
            }
            catch { }

            return report;
        }

        private List<MonthlyRevenueDTO> GenerateMonthlyRevenue()
        {
            var monthlyRevenue = new List<MonthlyRevenueDTO>();
            
            for (int i = 11; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                var monthName = date.ToString("MMM/yy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
                
                monthlyRevenue.Add(new MonthlyRevenueDTO
                {
                    Month = monthName,
                    ExpectedRevenue = 0,
                    ReceivedRevenue = 0,
                    Difference = 0
                });
            }

            return monthlyRevenue;
        }

        private string CalculatePerformanceRating(decimal occupancyRate, decimal netProfit)
        {
            if (occupancyRate >= 80 && netProfit > 1000)
                return "Excelente";
            else if (occupancyRate >= 60 && netProfit > 500)
                return "Bom";
            else if (occupancyRate >= 40 && netProfit > 0)
                return "Regular";
            else
                return "Crítico";
        }
    }
}
