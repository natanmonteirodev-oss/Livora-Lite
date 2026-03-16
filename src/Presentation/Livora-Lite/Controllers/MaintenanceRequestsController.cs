using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Livora_Lite.Controllers
{
    [Authorize]
    public class MaintenanceRequestsController : Controller
    {
        private readonly IMaintenanceRequestService _maintenanceRequestService;
        private readonly IPropertyService _propertyService;
        private readonly IContractService _contractService;
        private readonly IAuditService _auditService;

        public MaintenanceRequestsController(
            IMaintenanceRequestService maintenanceRequestService,
            IPropertyService propertyService,
            IContractService contractService,
            IAuditService auditService)
        {
            _maintenanceRequestService = maintenanceRequestService;
            _propertyService = propertyService;
            _contractService = contractService;
            _auditService = auditService;
        }

        // GET: MaintenanceRequests
        public async Task<IActionResult> Index()
        {
            var maintenanceRequests = await _maintenanceRequestService.GetAllMaintenanceRequestsAsync();
            return View(maintenanceRequests);
        }

        // GET: MaintenanceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _maintenanceRequestService.GetMaintenanceRequestByIdAsync(id.Value);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: MaintenanceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMaintenanceRequestDTO maintenanceRequest)
        {
            if (ModelState.IsValid)
            {
                var requestId = await _maintenanceRequestService.CreateMaintenanceRequestAsync(maintenanceRequest);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "MaintenanceRequest",
                    requestId.ToString(),
                    $"Criada solicitação de manutenção: {maintenanceRequest.Title} para propriedade ID: {maintenanceRequest.PropertyId}"
                );
                
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _maintenanceRequestService.GetMaintenanceRequestByIdAsync(id.Value);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateMaintenanceRequestDTO
            {
                Id = maintenanceRequest.Id,
                PropertyId = maintenanceRequest.PropertyId,
                ContractId = maintenanceRequest.ContractId,
                Description = maintenanceRequest.Description,
                RequestDate = maintenanceRequest.RequestDate,
                Priority = maintenanceRequest.Priority,
                Status = maintenanceRequest.Status
            };

            await PopulateDropdowns();
            return View(updateDto);
        }

        // POST: MaintenanceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateMaintenanceRequestDTO maintenanceRequest)
        {
            if (id != maintenanceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _maintenanceRequestService.UpdateMaintenanceRequestAsync(maintenanceRequest);
                    
                    // Log audit
                    var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                    var userName = User.Identity?.Name ?? "Unknown";
                    await _auditService.LogActionAsync(
                        userId,
                        userName,
                        "Update",
                        "MaintenanceRequest",
                        maintenanceRequest.Id.ToString(),
                        $"Atualizada solicitação de manutenção ID: {maintenanceRequest.Id}"
                    );
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _maintenanceRequestService.GetMaintenanceRequestByIdAsync(id.Value);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var request = await _maintenanceRequestService.GetMaintenanceRequestByIdAsync(id);
                var requestInfo = request != null ? $"Solicitação de manutenção: {request.Title}" : $"Solicitação ID: {id}";
                
                await _maintenanceRequestService.DeleteMaintenanceRequestAsync(id);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Delete",
                    "MaintenanceRequest",
                    id.ToString(),
                    $"Deletada {requestInfo}"
                );
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns()
        {
            var properties = await _propertyService.GetAllAsync();
            ViewBag.PropertyId = new SelectList(properties ?? new List<PropertyDTO>(), "Id", "Name");

            var contracts = await _contractService.GetAllContractsAsync();
            ViewBag.ContractId = new SelectList(contracts ?? new List<ContractDTO>(), "Id", "Id", true); // Allow null

            ViewBag.Priority = new SelectList(Enum.GetValues(typeof(MaintenancePriority))
                .Cast<MaintenancePriority>()
                .Select(p => new { Value = (int)p, Text = p.ToString() }), "Value", "Text");

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(MaintenanceStatus))
                .Cast<MaintenanceStatus>()
                .Select(s => new { Value = (int)s, Text = s.ToString() }), "Value", "Text");
        }
    }
}