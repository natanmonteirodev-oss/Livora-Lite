using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    // GET: Reports
    public IActionResult Index()
    {
        return View();
    }

    // GET: Reports/Financial
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Financial()
    {
        try
        {
            var report = await _reportService.GetFinancialReportAsync();
            return View(report);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao gerar relatório financeiro: {ex.Message}");
            return View("Error");
        }
    }

    // GET: Reports/PropertyPerformance
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> PropertyPerformance()
    {
        try
        {
            var report = await _reportService.GetPropertyPerformanceReportAsync();
            return View(report);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao gerar relatório de propriedades: {ex.Message}");
            return View("Error");
        }
    }

    // GET: Reports/ContractAnalysis
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> ContractAnalysis()
    {
        try
        {
            var report = await _reportService.GetContractAnalysisReportAsync();
            return View(report);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao gerar relatório de contratos: {ex.Message}");
            return View("Error");
        }
    }

    // GET: Reports/Maintenance
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Maintenance()
    {
        try
        {
            var report = await _reportService.GetMaintenanceReportAsync();
            return View(report);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao gerar relatório de manutenção: {ex.Message}");
            return View("Error");
        }
    }
}
