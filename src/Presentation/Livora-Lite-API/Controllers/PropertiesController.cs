using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Propriedades - Gerencia consulta, criação, atualização e exclusão de propriedades imobiliárias
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IAuditService _auditService;

        public PropertiesController(IPropertyService propertyService, IAuditService auditService)
        {
            _propertyService = propertyService;
            _auditService = auditService;
        }

        /// <summary>
        /// Lista todas as propriedades
        /// </summary>
        /// <returns>Lista de propriedades</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAll()
        {
            var properties = await _propertyService.GetAllAsync();
            return Ok(properties);
        }

        /// <summary>
        /// Obtém uma propriedade específica pelo ID
        /// </summary>
        /// <param name="id">ID da propriedade</param>
        /// <returns>Detalhes da propriedade</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyDTO>> GetById(int id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            return Ok(property);
        }

        /// <summary>
        /// Cria uma nova propriedade
        /// </summary>
        /// <param name="request">Dados da propriedade a ser criada</param>
        /// <returns>ID e mensagem de confirmação da propriedade criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreatePropertyRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get OwnerId from authenticated user
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int ownerId))
            {
                return BadRequest(new { message = "Erro ao obter informações do usuário autenticado." });
            }

            request.OwnerId = ownerId;

            try
            {
                var property = await _propertyService.CreateAsync(request);
                
                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "Property",
                    property.Id.ToString(),
                    $"Propriedade '{property.Name}' foi criada"
                );

                return Ok(new { id = property.Id, message = "Propriedade criada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar propriedade: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma propriedade existente
        /// </summary>
        /// <param name="id">ID da propriedade a ser atualizada</param>
        /// <param name="request">Novos dados da propriedade</param>
        /// <returns>Mensagem de confirmação da atualização</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var property = await _propertyService.UpdateAsync(request);
                
                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Update",
                    "Property",
                    property.Id.ToString(),
                    $"Propriedade '{property.Name}' foi alterada"
                );

                return Ok(new { message = "Propriedade atualizada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar propriedade: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta uma propriedade
        /// </summary>
        /// <param name="id">ID da propriedade a ser deletada</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _propertyService.DeleteAsync(id);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Delete",
                    "Property",
                    id.ToString(),
                    $"Propriedade ID {id} foi deletada"
                );

                return Ok(new { message = "Propriedade deletada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar propriedade: " + ex.Message });
            }
        }
    }
}
