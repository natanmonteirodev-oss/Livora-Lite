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

    public PropertiesController(
        IPropertyService propertyService,
        IPropertyTypeRepository propertyTypeRepository,
        IPropertyStatusRepository propertyStatusRepository)
    {
        _propertyService = propertyService;
        _propertyTypeRepository = propertyTypeRepository;
        _propertyStatusRepository = propertyStatusRepository;
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
        if (ModelState.IsValid)
        {
            await _propertyService.CreateAsync(request);
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
            await _propertyService.UpdateAsync(request);
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
        await _propertyService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}