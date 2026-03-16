using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using Livora_Lite.Application.DTO;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Controllers;

[Authorize]
public class ContractsController : Controller
{
    private readonly IContractService _contractService;
    private readonly IPropertyRepository _propertyRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IContractStatusRepository _contractStatusRepository;
    private readonly IAuditService _auditService;

    public ContractsController(
        IContractService contractService,
        IPropertyRepository propertyRepository,
        ITenantRepository tenantRepository,
        IContractStatusRepository contractStatusRepository,
        IAuditService auditService)
    {
        _contractService = contractService;
        _propertyRepository = propertyRepository;
        _tenantRepository = tenantRepository;
        _contractStatusRepository = contractStatusRepository;
        _auditService = auditService;
    }

    // GET: Contracts
    public async Task<IActionResult> Index()
    {
        var contracts = await _contractService.GetAllContractsAsync();
        return View(contracts);
    }

    // GET: Contracts/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        if (contract == null)
        {
            return NotFound();
        }
        return View(contract);
    }

    // GET: Contracts/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Properties = await _propertyRepository.GetAllAsync();
        ViewBag.Tenants = await _tenantRepository.GetAllAsync();
        ViewBag.ContractStatuses = await _contractStatusRepository.GetAllAsync();
        return View();
    }

    // POST: Contracts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContractRequestDTO request)
    {
        if (ModelState.IsValid)
        {
            var contractId = await _contractService.CreateAsync(request);
            
            // Log audit
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
            var userName = User.Identity?.Name ?? "Unknown";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Create",
                "Contract",
                contractId.ToString(),
                $"Criado contrato para propriedade ID: {request.PropertyId}, inquilino ID: {request.TenantId}"
            );
            
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Properties = await _propertyRepository.GetAllAsync();
        ViewBag.Tenants = await _tenantRepository.GetAllAsync();
        ViewBag.ContractStatuses = await _contractStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Contracts/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        if (contract == null)
        {
            return NotFound();
        }
        ViewBag.Properties = await _propertyRepository.GetAllAsync();
        ViewBag.Tenants = await _tenantRepository.GetAllAsync();
        ViewBag.ContractStatuses = await _contractStatusRepository.GetAllAsync();

        var updateRequest = new UpdateContractRequestDTO
        {
            Id = contract.Id,
            PropertyId = contract.PropertyId,
            TenantId = contract.TenantId,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            RentValue = contract.RentValue,
            DueDay = contract.DueDay,
            LateFee = contract.LateFee,
            InterestRate = contract.InterestRate,
            SecurityDeposit = contract.SecurityDeposit,
            ContractStatusId = contract.ContractStatusId
        };

        return View(updateRequest);
    }

    // POST: Contracts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateContractRequestDTO request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _contractService.UpdateAsync(request);
            
            // Log audit
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
            var userName = User.Identity?.Name ?? "Unknown";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Update",
                "Contract",
                request.Id.ToString(),
                $"Atualizado contrato ID: {request.Id}"
            );
            
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Properties = await _propertyRepository.GetAllAsync();
        ViewBag.Tenants = await _tenantRepository.GetAllAsync();
        ViewBag.ContractStatuses = await _contractStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Contracts/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        if (contract == null)
        {
            return NotFound();
        }
        return View(contract);
    }

    // POST: Contracts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        var contractInfo = contract != null ? $"Contrato ID: {contract.Id}" : $"Contrato ID: {id}";
        
        await _contractService.DeleteAsync(id);
        
        // Log audit
        var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
        var userName = User.Identity?.Name ?? "Unknown";
        await _auditService.LogActionAsync(
            userId,
            userName,
            "Delete",
            "Contract",
            id.ToString(),
            $"Deletado {contractInfo}"
        );
        
        return RedirectToAction(nameof(Index));
    }
}