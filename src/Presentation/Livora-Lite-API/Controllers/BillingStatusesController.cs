using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Status de Cobrança - Fornece acesso aos status de cobranças disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BillingStatusesController : ControllerBase
    {
        private readonly IBillingStatusRepository _billingStatusRepository;

        public BillingStatusesController(IBillingStatusRepository billingStatusRepository)
        {
            _billingStatusRepository = billingStatusRepository;
        }

        /// <summary>
        /// Lista todos os status de cobranças
        /// </summary>
        /// <returns>Lista de status de cobranças</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _billingStatusRepository.GetAllAsync();
            return Ok(statuses);
        }

        /// <summary>
        /// Obtém um status de cobrança específico pelo ID
        /// </summary>
        /// <param name="id">ID do status de cobrança</param>
        /// <returns>Detalhes do status de cobrança</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _billingStatusRepository.GetByIdAsync(id);
            if (status == null)
                return NotFound();

            return Ok(status);
        }
    }
}
