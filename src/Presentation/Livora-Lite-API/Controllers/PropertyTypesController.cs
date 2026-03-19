using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Tipos de Propriedade - Fornece acesso aos tipos de propriedades disponíveis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyTypesController(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        /// <summary>
        /// Lista todos os tipos de propriedades
        /// </summary>
        /// <returns>Lista de tipos de propriedades</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var types = await _propertyTypeRepository.GetAllAsync();
            return Ok(types);
        }

        /// <summary>
        /// Obtém um tipo de propriedade específico pelo ID
        /// </summary>
        /// <param name="id">ID do tipo de propriedade</param>
        /// <returns>Detalhes do tipo de propriedade</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _propertyTypeRepository.GetByIdAsync(id);
            if (type == null)
                return NotFound();

            return Ok(type);
        }
    }
}
