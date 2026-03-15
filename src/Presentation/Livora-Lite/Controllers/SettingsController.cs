using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Controllers;

[Authorize] // Apenas usuários logados
public class SettingsController : Controller
{
    private readonly IAppSettingsRepository _appSettingsRepository;

    public SettingsController(IAppSettingsRepository appSettingsRepository)
    {
        _appSettingsRepository = appSettingsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Verificar se é admin - por enquanto, assumir que todos logados são admins
        // TODO: Implementar role-based auth

        var timeoutSetting = await _appSettingsRepository.GetByKeyAsync("SessionTimeoutMinutes");
        ViewBag.SessionTimeout = timeoutSetting?.Value ?? "5";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTimeout(int sessionTimeoutMinutes)
    {
        if (sessionTimeoutMinutes < 1 || sessionTimeoutMinutes > 1440) // 1 min to 24 hours
        {
            ModelState.AddModelError("", "Timeout deve ser entre 1 e 1440 minutos.");
            ViewBag.SessionTimeout = sessionTimeoutMinutes.ToString();
            return View("Index");
        }

        var setting = new Domain.Entities.AppSettings
        {
            Key = "SessionTimeoutMinutes",
            Value = sessionTimeoutMinutes.ToString()
        };

        await _appSettingsRepository.CreateOrUpdateAsync(setting);

        TempData["Message"] = "Timeout de sessão atualizado com sucesso.";
        ViewBag.SessionTimeout = sessionTimeoutMinutes.ToString();
        return View("Index");
    }
}