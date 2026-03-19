using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Pagamentos - Gerencia consulta, criação, atualização e exclusão de pagamentos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBillingService _billingService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAuditService _auditService;

        public PaymentsController(
            IPaymentService paymentService,
            IBillingService billingService,
            IPaymentMethodService paymentMethodService,
            IAuditService auditService)
        {
            _paymentService = paymentService;
            _billingService = billingService;
            _paymentMethodService = paymentMethodService;
            _auditService = auditService;
        }

        /// <summary>
        /// Lista todos os pagamentos
        /// </summary>
        /// <returns>Lista de pagamentos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAll()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        /// <summary>
        /// Obtém um pagamento específico pelo ID
        /// </summary>
        /// <param name="id">ID do pagamento</param>
        /// <returns>Detalhes do pagamento</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDTO>> GetById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        /// <summary>
        /// Cria um novo pagamento
        /// </summary>
        /// <param name="request">Dados do pagamento a ser criado</param>
        /// <returns>ID e mensagem de confirmação do pagamento criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var paymentId = await _paymentService.CreatePaymentAsync(request);

                // Log auditing
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "Payment",
                    paymentId.ToString() ?? "0",
                    $"Criado pagamento de R$ {request.Amount} para cobrança ID: {request.BillingId}"
                );

                return Ok(new { id = paymentId, message = "Pagamento criado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar pagamento: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um pagamento existente
        /// </summary>
        /// <param name="id">ID do pagamento a ser atualizado</param>
        /// <param name="request">Novos dados do pagamento</param>
        /// <returns>Mensagem de confirmação da atualização</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _paymentService.UpdatePaymentAsync(request);
                return Ok(new { message = "Pagamento atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar pagamento: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta um pagamento
        /// </summary>
        /// <param name="id">ID do pagamento a ser deletado</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return Ok(new { message = "Pagamento deletado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar pagamento: " + ex.Message });
            }
        }
    }
}
