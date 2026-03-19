using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Solicitações de Manutenção - Gerencia consulta, criação e exclusão de solicitações de manutenção
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaintenanceRequestsController : ControllerBase
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

        /// <summary>
        /// Lista todas as solicitações de manutenção
        /// </summary>
        /// <returns>Lista de solicitações de manutenção</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<MaintenanceRequestDTO>>> GetAll()
        {
            var maintenanceRequests = await _maintenanceRequestService.GetAllMaintenanceRequestsAsync();
            return Ok(maintenanceRequests);
        }

        /// <summary>
        /// Obtém uma solicitação de manutenção específica pelo ID
        /// </summary>
        /// <param name="id">ID da solicitação de manutenção</param>
        /// <returns>Detalhes da solicitação de manutenção</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MaintenanceRequestDTO>> GetById(int id)
        {
            var maintenanceRequest = await _maintenanceRequestService.GetMaintenanceRequestByIdAsync(id);
            if (maintenanceRequest == null)
                return NotFound();

            return Ok(maintenanceRequest);
        }

        /// <summary>
        /// Cria uma nova solicitação de manutenção
        /// </summary>
        /// <param name="request">Dados da solicitação de manutenção a ser criada</param>
        /// <returns>ID e mensagem de confirmação da solicitação criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateMaintenanceRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Auto-generate Title with format: PropertyName + ddmmyyyy
                var property = await _propertyService.GetByIdAsync(request.PropertyId);
                if (property != null)
                {
                    request.Title = $"{property.Name} - {DateTime.Now.ToString("ddMMyyyy")}";
                }
                else
                {
                    request.Title = DateTime.Now.ToString("ddMMyyyy");
                }

                var requestId = await _maintenanceRequestService.CreateMaintenanceRequestAsync(request);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "MaintenanceRequest",
                    requestId.ToString() ?? "0",
                    $"Criada solicitação de manutenção: {request.Title} para propriedade ID: {request.PropertyId}"
                );

                return Ok(new { id = requestId, message = "Solicitação de manutenção criada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar solicitação de manutenção: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta uma solicitação de manutenção
        /// </summary>
        /// <param name="id">ID da solicitação de manutenção a ser deletada</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _maintenanceRequestService.DeleteMaintenanceRequestAsync(id);
                return Ok(new { message = "Solicitação de manutenção deletada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar solicitação de manutenção: " + ex.Message });
            }
        }
    }
}
