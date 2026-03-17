using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Models;
using Microsoft.AspNetCore.Authorization;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        // Se usuário está autenticado, redirecionar para o Dashboard
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Dashboard");
        }

        // Caso contrário, mostrar a landing page
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            // Extrair userId da claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError(string.Empty, "Erro ao identificar usuário");
                return View("Index");
            }

            if (User.IsInRole("Admin"))
            {
                var adminDashboard = await _dashboardService.GetAdminDashboardAsync();
                return View("AdminDashboard", adminDashboard);
            }
            else if (User.IsInRole("Owner"))
            {
                var ownerDashboard = await _dashboardService.GetOwnerDashboardAsync(userId);
                return View("OwnerDashboard", ownerDashboard);
            }
            else // Home (Tenant)
            {
                var tenantDashboard = await _dashboardService.GetTenantDashboardAsync(userId);
                return View("TenantDashboard", tenantDashboard);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao carregar dashboard: {ex.Message}");
            return View("Index");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
