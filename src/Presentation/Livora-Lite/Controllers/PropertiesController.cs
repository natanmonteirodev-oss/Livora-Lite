using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using Livora_Lite.Application.DTO;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Controllers;

[Authorize]
public class PropertiesController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private readonly IPropertyStatusRepository _propertyStatusRepository;
    private readonly IAuditService _auditService;

    public PropertiesController(
        IPropertyService propertyService,
        IPropertyTypeRepository propertyTypeRepository,
        IPropertyStatusRepository propertyStatusRepository,
        IAuditService auditService)
    {
        _propertyService = propertyService;
        _propertyTypeRepository = propertyTypeRepository;
        _propertyStatusRepository = propertyStatusRepository;
        _auditService = auditService;
    }

    // GET: Properties
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetAllAsync();
        return View(properties);
    }

    // GET: Properties/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property == null)
        {
            return NotFound();
        }
        return View(property);
    }

    // GET: Properties/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
        ViewBag.PropertyStatuses = await _propertyStatusRepository.GetAllAsync();
        return View();
    }

    // POST: Properties/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePropertyRequestDTO request)
    {
        // Obter OwnerId do usuário autenticado
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int ownerId))
        {
            ModelState.AddModelError("", "Erro ao obter informações do usuário autenticado.");
            ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
            ViewBag.PropertyStatuses = await _propertyStatusRepository.GetAllAsync();
            return View(request);
        }

        // Atribuir OwnerId ao DTO
        request.OwnerId = ownerId;

        if (ModelState.IsValid)
        {
            var property = await _propertyService.CreateAsync(request);
            
            // Registrar auditoria
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Sistema";
            var userName = User.Identity?.Name ?? "Sistema";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Criado",
                "Property",
                property.Id.ToString(),
                $"Propriedade '{property.Name}' foi criada"
            );
            
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
        ViewBag.PropertyStatuses = await _propertyStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Properties/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property == null)
        {
            return NotFound();
        }
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
        ViewBag.PropertyStatuses = await _propertyStatusRepository.GetAllAsync();

        var updateRequest = new UpdatePropertyRequestDTO
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address != null ? new UpdateAddressRequestDTO
            {
                Id = property.Address.Id,
                Street = property.Address.Street,
                Number = property.Address.Number,
                Complement = property.Address.Complement,
                Neighborhood = property.Address.Neighborhood,
                City = property.Address.City,
                State = property.Address.State,
                ZipCode = property.Address.ZipCode,
                Country = property.Address.Country
            } : new UpdateAddressRequestDTO(),
            PropertyTypeId = property.PropertyType?.Id ?? 0,
            PropertyStatusId = property.PropertyStatus?.Id ?? 0,
            Area = property.Area,
            Bedrooms = property.Bedrooms,
            Bathrooms = property.Bathrooms,
            ParkingSpaces = property.ParkingSpaces,
            SuggestedRentValue = property.SuggestedRentValue,
            Description = property.Description
        };

        return View(updateRequest);
    }

    // POST: Properties/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdatePropertyRequestDTO request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var property = await _propertyService.UpdateAsync(request);
            
            // Registrar auditoria
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Sistema";
            var userName = User.Identity?.Name ?? "Sistema";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Alterado",
                "Property",
                property.Id.ToString(),
                $"Propriedade '{property.Name}' foi alterada"
            );
            
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
        ViewBag.PropertyStatuses = await _propertyStatusRepository.GetAllAsync();
        return View(request);
    }

    // GET: Properties/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property == null)
        {
            return NotFound();
        }
        return View(property);
    }

    // POST: Properties/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property != null)
        {
            // Registrar auditoria antes de excluir
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Sistema";
            var userName = User.Identity?.Name ?? "Sistema";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Excluído",
                "Property",
                property.Id.ToString(),
                $"Propriedade '{property.Name}' foi excluída"
            );
        }
        
        await _propertyService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}