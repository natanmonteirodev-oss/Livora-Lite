using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Contratos - Gerencia consulta, criação, atualização e exclusão de contratos de aluguel
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IPropertyService _propertyService;
        private readonly ITenantService _tenantService;
        private readonly IContractStatusRepository _contractStatusRepository;
        private readonly IAuditService _auditService;

        public ContractsController(
            IContractService contractService,
            IPropertyService propertyService,
            ITenantService tenantService,
            IContractStatusRepository contractStatusRepository,
            IAuditService auditService)
        {
            _contractService = contractService;
            _propertyService = propertyService;
            _tenantService = tenantService;
            _contractStatusRepository = contractStatusRepository;
            _auditService = auditService;
        }

        /// <summary>
        /// Lista todos os contratos
        /// </summary>
        /// <returns>Lista de contratos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ContractDTO>>> GetAll()
        {
            var contracts = await _contractService.GetAllContractsAsync();
            return Ok(contracts);
        }

        /// <summary>
        /// Obtém um contrato específico pelo ID
        /// </summary>
        /// <param name="id">ID do contrato</param>
        /// <returns>Detalhes do contrato</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContractDTO>> GetById(int id)
        {
            var contract = await _contractService.GetByIdAsync(id);
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        /// <summary>
        /// Cria um novo contrato
        /// </summary>
        /// <param name="request">Dados do contrato a ser criado</param>
        /// <returns>ID e mensagem de confirmação do contrato criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateContractRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contractId = await _contractService.CreateAsync(request);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "Contract",
                    contractId.ToString() ?? "0",
                    $"Criado contrato para propriedade ID: {request.PropertyId}, inquilino ID: {request.TenantId}"
                );

                return Ok(new { id = contractId, message = "Contrato criado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "Erro de validação: " + ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar contrato: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um contrato existente
        /// </summary>
        /// <param name="id">ID do contrato a ser atualizado</param>
        /// <param name="request">Novos dados do contrato</param>
        /// <returns>Mensagem de confirmação da atualização</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateContractRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _contractService.UpdateAsync(request);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Update",
                    "Contract",
                    request.Id.ToString(),
                    $"Atualizado contrato ID: {request.Id}"
                );

                return Ok(new { message = "Contrato atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar contrato: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta um contrato
        /// </summary>
        /// <param name="id">ID do contrato a ser deletado</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _contractService.DeleteAsync(id);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Delete",
                    "Contract",
                    id.ToString(),
                    $"Contrato ID {id} foi deletado"
                );

                return Ok(new { message = "Contrato deletado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar contrato: " + ex.Message });
            }
        }
    }
}
