using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Inquilinos - Gerencia consulta, criação, atualização e exclusão de inquilinos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TenantsController : ControllerBase
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

        /// <summary>
        /// Lista todos os inquilinos
        /// </summary>
        /// <returns>Lista de inquilinos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TenantDTO>>> GetAll()
        {
            var tenants = await _tenantService.GetAllAsync();
            return Ok(tenants);
        }

        /// <summary>
        /// Obtém um inquilino específico pelo ID
        /// </summary>
        /// <param name="id">ID do inquilino</param>
        /// <returns>Detalhes do inquilino</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TenantDTO>> GetById(int id)
        {
            var tenant = await _tenantService.GetByIdAsync(id);
            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        /// <summary>
        /// Cria um novo inquilino
        /// </summary>
        /// <param name="request">Dados do inquilino a ser criado</param>
        /// <returns>ID e mensagem de confirmação do inquilino criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateTenantRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get UserId from authenticated user
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { message = "Erro ao obter informações do usuário autenticado." });
            }

            request.UserId = userId;

            // Validate TenantStatusId if provided
            if (request.TenantStatusId > 0)
            {
                var statusExists = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);
                if (statusExists == null)
                {
                    return BadRequest(new { message = "O status selecionado é inválido." });
                }
            }

            try
            {
                var tenantId = await _tenantService.CreateAsync(request);

                // Log auditing
                var userId_audit = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId_audit,
                    userName,
                    "Create",
                    "Tenant",
                    tenantId.ToString() ?? "0",
                    $"Criado inquilino: {request.FirstName} {request.LastName}"
                );

                return Ok(new { id = tenantId, message = "Inquilino criado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar inquilino: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um inquilino existente
        /// </summary>
        /// <param name="id">ID do inquilino a ser atualizado</param>
        /// <param name="request">Novos dados do inquilino</param>
        /// <returns>Mensagem de confirmação da atualização</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate TenantStatusId if provided
            if (request.TenantStatusId > 0)
            {
                var statusExists = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);
                if (statusExists == null)
                {
                    return BadRequest(new { message = "O status selecionado é inválido." });
                }
            }

            try
            {
                await _tenantService.UpdateAsync(request);

                // Log auditing
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

                return Ok(new { message = "Inquilino atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar inquilino: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta um inquilino
        /// </summary>
        /// <param name="id">ID do inquilino a ser deletado</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tenantService.DeleteAsync(id);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Delete",
                    "Tenant",
                    id.ToString(),
                    $"Inquilino ID {id} foi deletado"
                );

                return Ok(new { message = "Inquilino deletado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar inquilino: " + ex.Message });
            }
        }
    }
}
