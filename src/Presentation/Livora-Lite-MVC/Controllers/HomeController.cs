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

    [Authorize]
    public async Task<IActionResult> DebugDashboard()
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

            object dashboardData = new { };

            if (User.IsInRole("Admin"))
            {
                dashboardData = await _dashboardService.GetAdminDashboardAsync();
            }
            else if (User.IsInRole("Owner"))
            {
                dashboardData = await _dashboardService.GetOwnerDashboardAsync(userId);
            }
            else // Tenant
            {
                dashboardData = await _dashboardService.GetTenantDashboardAsync(userId);
            }

            return View("DebugDashboard", dashboardData);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao carregar dashboard: {ex.Message}");
            return View("Index");
        }
    }

    [Authorize]
    public async Task<IActionResult> ApiDebugDashboard()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { error = "Erro ao identificar usuário", timestamp = DateTime.UtcNow });
            }

            if (User.IsInRole("Admin"))
            {
                var adminDashboard = await _dashboardService.GetAdminDashboardAsync();
                return Json(new 
                { 
                    success = true,
                    role = "Admin",
                    userId = userId,
                    timestamp = DateTime.UtcNow,
                    data = new
                    {
                        adminDashboard.TotalUsers,
                        adminDashboard.TotalProperties,
                        adminDashboard.TotalTenants,
                        adminDashboard.TotalContracts,
                        adminDashboard.TotalContracts_Active,
                        adminDashboard.TotalContracts_Expired,
                        adminDashboard.TotalRevenueExpected,
                        adminDashboard.PendingPayments,
                        adminDashboard.PendingPaymentsAmount,
                        adminDashboard.PendingMaintenance,
                        topPropertiesCount = adminDashboard.TopProperties?.Count ?? 0,
                        recentActivitiesCount = adminDashboard.RecentActivities?.Count ?? 0
                    }
                });
            }
            else if (User.IsInRole("Owner"))
            {
                var ownerDashboard = await _dashboardService.GetOwnerDashboardAsync(userId);
                return Json(new 
                { 
                    success = true,
                    role = "Owner",
                    userId = userId,
                    timestamp = DateTime.UtcNow,
                    data = ownerDashboard
                });
            }
            else
            {
                var tenantDashboard = await _dashboardService.GetTenantDashboardAsync(userId);
                return Json(new 
                { 
                    success = true,
                    role = "Tenant",
                    userId = userId,
                    timestamp = DateTime.UtcNow,
                    data = tenantDashboard
                });
            }
        }
        catch (Exception ex)
        {
            // Log to try to understand the error
            System.IO.File.AppendAllText(
                "C:\\Users\\natan\\repos\\Livora-Lite\\debug.log",
                $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Exception: {ex.Message}\n{ex.StackTrace}\n\n"
            );

            return Json(new 
            { 
                success = false, 
                error = ex.Message,
                exceptionType = ex.GetType().Name,
                timestamp = DateTime.UtcNow
            });
        }
    }

    public IActionResult TestChart()
    {
        return View();
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
