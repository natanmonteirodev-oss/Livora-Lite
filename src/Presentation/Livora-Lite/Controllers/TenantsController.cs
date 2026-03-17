using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using Livora_Lite.Application.DTO;
using Livora_Lite.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Controllers;

[Authorize]
public class TenantsController : Controller
{
    private readonly ITenantService _tenantService;
    private readonly ITenantStatusRepository _tenantStatusRepository;
    private readonly IAuditService _auditService;

    public TenantsController(
        ITenantService tenantService,
        ITenantStatusRepository tenantStatusRepository,
        IAuditService auditService)
    {
        _tenantService = tenantService;
        _tenantStatusRepository = tenantStatusRepository;
        _auditService = auditService;
    }

    // GET: Tenants
    public async Task<IActionResult> Index()
    {
        var tenants = await _tenantService.GetAllAsync();
        return View(tenants);
    }

    // GET: Tenants/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var tenant = await _tenantService.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        return View(tenant);
    }

    // GET: Tenants/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();
        return View();
    }

    // POST: Tenants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTenantRequestDTO request)
    {
        // Validação proativa: verificar se o TenantStatusId existe
        if (request.TenantStatusId > 0)
        {
            var statusExists = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);
            if (statusExists == null)
            {
                ModelState.AddModelError(nameof(request.TenantStatusId), "O status selecionado é inválido. Por favor, selecione um status válido.");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                var tenantId = await _tenantService.CreateAsync(request);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "Tenant",
                    tenantId.ToString() ?? "Unknown",
                    $"Criado inquilino: {request.FirstName} {request.LastName}"
                );
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar inquilino: {ex.Message}");
                ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();
                return View(request);
            }
        }
        
        ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Tenants/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tenant = await _tenantService.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();

        var updateRequest = new UpdateTenantRequestDTO
        {
            Id = tenant.Id,
            FirstName = tenant.FirstName,
            LastName = tenant.LastName,
            Document = tenant.Document,
            Phone = tenant.Phone,
            Email = tenant.Email,
            CurrentAddress = tenant.CurrentAddress,
            TenantStatusId = tenant.TenantStatus?.Id ?? 0
        };

        return View(updateRequest);
    }

    // POST: Tenants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateTenantRequestDTO request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }

        // Validação proativa: verificar se o TenantStatusId existe
        if (request.TenantStatusId > 0)
        {
            var statusExists = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);
            if (statusExists == null)
            {
                ModelState.AddModelError(nameof(request.TenantStatusId), "O status selecionado é inválido. Por favor, selecione um status válido.");
            }
        }
        else
        {
            ModelState.AddModelError(nameof(request.TenantStatusId), "Selecione um status válido para o inquilino.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _tenantService.UpdateAsync(request);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Update",
                    "Tenant",
                    request.Id.ToString(),
                    $"Atualizado inquilino: {request.FirstName} {request.LastName}"
                );
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar inquilino: {ex.Message}");
                ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();
                return View(request);
            }
        }
        ViewBag.TenantStatuses = await _tenantStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Tenants/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tenant = await _tenantService.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        return View(tenant);
    }

    // POST: Tenants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tenant = await _tenantService.GetByIdAsync(id);
        var tenantName = tenant != null ? $"{tenant.FirstName} {tenant.LastName}" : "Unknown";
        
        await _tenantService.DeleteAsync(id);
        
        // Log audit
        var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
        var userName = User.Identity?.Name ?? "Unknown";
        await _auditService.LogActionAsync(
            userId,
            userName,
            "Delete",
            "Tenant",
            id.ToString(),
            $"Deletado inquilino: {tenantName}"
        );
        
        return RedirectToAction(nameof(Index));
    }
}