using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Status de Inquilino - Fornece acesso aos status de inquilinos disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TenantStatusesController : ControllerBase
    {
        private readonly ITenantStatusRepository _tenantStatusRepository;

        public TenantStatusesController(ITenantStatusRepository tenantStatusRepository)
        {
            _tenantStatusRepository = tenantStatusRepository;
        }

        /// <summary>
        /// Lista todos os status de inquilinos
        /// </summary>
        /// <returns>Lista de status de inquilinos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _tenantStatusRepository.GetAllAsync();
            return Ok(statuses);
        }

        /// <summary>
        /// Obtém um status de inquilino específico pelo ID
        /// </summary>
        /// <param name="id">ID do status de inquilino</param>
        /// <returns>Detalhes do status de inquilino</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _tenantStatusRepository.GetByIdAsync(id);
            if (status == null)
                return NotFound();

            return Ok(status);
        }
    }
}
