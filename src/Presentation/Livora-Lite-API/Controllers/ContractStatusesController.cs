using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Status de Contrato - Fornece acesso aos status de contratos disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractStatusesController : ControllerBase
    {
        private readonly IContractStatusRepository _contractStatusRepository;

        public ContractStatusesController(IContractStatusRepository contractStatusRepository)
        {
            _contractStatusRepository = contractStatusRepository;
        }

        /// <summary>
        /// Lista todos os status de contratos
        /// </summary>
        /// <returns>Lista de status de contratos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _contractStatusRepository.GetAllAsync();
            return Ok(statuses);
        }

        /// <summary>
        /// Obtém um status de contrato específico pelo ID
        /// </summary>
        /// <param name="id">ID do status de contrato</param>
        /// <returns>Detalhes do status de contrato</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _contractStatusRepository.GetByIdAsync(id);
            if (status == null)
                return NotFound();

            return Ok(status);
        }
    }
}
