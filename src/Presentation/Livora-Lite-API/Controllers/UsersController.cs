using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Usuários
    /// Gerencia consulta, criação, atualização e exclusão de usuários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtém todos os usuários
        /// </summary>
        /// <returns>Lista de todos os usuários</returns>
        /// <response code="200">Listagem de usuários retornada com sucesso</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtém um usuário específico por ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Dados do usuário solicitado</returns>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="request">Dados do novo usuário</param>
        /// <returns>Confirmação da criação</returns>
        /// <response code="200">Usuário criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Create([FromBody] CreateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.CreateUserAsync(request);
                return Ok(new { message = "Usuário criado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar usuário: " + ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário a atualizar</param>
        /// <param name="request">Novos dados do usuário</param>
        /// <returns>Confirmação da atualização</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="401">Não autorizado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDTO request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.UpdateUserAsync(request);
                return Ok(new { message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar usuário: " + ex.Message });
            }
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado</param>
        /// <returns>Confirmação da deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "Usuário deletado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar usuário: " + ex.Message });
            }
        }
    }
}
