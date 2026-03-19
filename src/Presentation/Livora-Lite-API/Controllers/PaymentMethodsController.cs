using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Métodos de Pagamento - Fornece acesso aos métodos de pagamento disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Lista todos os métodos de pagamento
        /// </summary>
        /// <returns>Lista de métodos de pagamento</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PaymentMethodDTO>>> GetAll()
        {
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
            return Ok(paymentMethods);
        }

        /// <summary>
        /// Obtém um método de pagamento específico pelo ID
        /// </summary>
        /// <param name="id">ID do método de pagamento</param>
        /// <returns>Detalhes do método de pagamento</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentMethodDTO>> GetById(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
                return NotFound();

            return Ok(paymentMethod);
        }
    }
}
