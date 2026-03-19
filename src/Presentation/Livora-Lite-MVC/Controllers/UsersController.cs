using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _userService.CreateUserAsync(request);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar usuário: " + ex.Message);
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var editModel = new UpdateUserRequestDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _userService.UpdateUserAsync(request);
                TempData["Success"] = "Usuário atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar usuário: " + ex.Message);
                TempData["Error"] = ex.Message;
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao remover usuário: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
