using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _authService.LoginAsync(request);
        
        if (result.Success && result.User != null)
        {
            // TODO: Implementar autenticação com cookies ou JWT
            // Por enquanto, apenas redirecionar com sucesso
            HttpContext.Session.SetString("UserId", result.User.Id.ToString());
            HttpContext.Session.SetString("UserEmail", result.User.Email);
            HttpContext.Session.SetString("UserName", result.User.FullName);
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(request);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _authService.RegisterAsync(request);
        
        if (result.Success && result.User != null)
        {
            // TODO: Implementar autenticação com cookies ou JWT
            HttpContext.Session.SetString("UserId", result.User.Id.ToString());
            HttpContext.Session.SetString("UserEmail", result.User.Email);
            HttpContext.Session.SetString("UserName", result.User.FullName);
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(request);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
