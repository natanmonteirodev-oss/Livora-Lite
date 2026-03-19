using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Status de Propriedade - Fornece acesso aos status de propriedades disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertyStatusesController : ControllerBase
    {
        private readonly IPropertyStatusRepository _propertyStatusRepository;

        public PropertyStatusesController(IPropertyStatusRepository propertyStatusRepository)
        {
            _propertyStatusRepository = propertyStatusRepository;
        }

        /// <summary>
        /// Lista todos os status de propriedades
        /// </summary>
        /// <returns>Lista de status de propriedades</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _propertyStatusRepository.GetAllAsync();
            return Ok(statuses);
        }

        /// <summary>
        /// Obtém um status de propriedade específico pelo ID
        /// </summary>
        /// <param name="id">ID do status de propriedade</param>
        /// <returns>Detalhes do status de propriedade</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _propertyStatusRepository.GetByIdAsync(id);
            if (status == null)
                return NotFound();

            return Ok(status);
        }
    }
}
