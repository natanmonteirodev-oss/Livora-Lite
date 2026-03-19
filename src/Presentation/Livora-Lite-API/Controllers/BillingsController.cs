using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Cobranças - Gerencia consulta, criação, atualização e exclusão de cobranças
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BillingsController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly IContractService _contractService;
        private readonly IBillingStatusService _billingStatusService;

        public BillingsController(
            IBillingService billingService,
            IContractService contractService,
            IBillingStatusService billingStatusService)
        {
            _billingService = billingService;
            _contractService = contractService;
            _billingStatusService = billingStatusService;
        }

        /// <summary>
        /// Lista todas as cobranças
        /// </summary>
        /// <returns>Lista de cobranças</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<BillingDTO>>> GetAll()
        {
            var billings = await _billingService.GetAllBillingsAsync();
            return Ok(billings);
        }

        /// <summary>
        /// Obtém uma cobrança específica pelo ID
        /// </summary>
        /// <param name="id">ID da cobrança</param>
        /// <returns>Detalhes da cobrança</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BillingDTO>> GetById(int id)
        {
            var billing = await _billingService.GetBillingByIdAsync(id);
            if (billing == null)
                return NotFound();

            return Ok(billing);
        }

        /// <summary>
        /// Cria uma nova cobrança
        /// </summary>
        /// <param name="request">Dados da cobrança a ser criada</param>
        /// <returns>Mensagem de confirmação da cobrança criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateBillingRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _billingService.CreateBillingAsync(request);
                return Ok(new { message = "Cobrança criada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar cobrança: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma cobrança existente
        /// </summary>
        /// <param name="id">ID da cobrança a ser atualizada</param>
        /// <param name="request">Novos dados da cobrança</param>
        /// <returns>Mensagem de confirmação da atualização</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBillingRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _billingService.UpdateBillingAsync(request);
                return Ok(new { message = "Cobrança atualizada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar cobrança: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta uma cobrança
        /// </summary>
        /// <param name="id">ID da cobrança a ser deletada</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _billingService.DeleteBillingAsync(id);
                return Ok(new { message = "Cobrança deletada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar cobrança: " + ex.Message });
            }
        }
    }
}
