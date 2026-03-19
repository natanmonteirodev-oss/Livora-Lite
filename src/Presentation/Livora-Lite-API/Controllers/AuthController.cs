using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Autenticação
    /// Responsável por login e registro de usuários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Realiza o login do usuário
        /// </summary>
        /// <param name="request">Credenciais do usuário (email e senha)</param>
        /// <returns>Token JWT e informações do usuário</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);
            
            if (!result.Success || result.User == null || result.Token == null)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="request">Dados do novo usuário</param>
        /// <returns>Token JWT e informações do usuário criado</returns>
        /// <response code="200">Usuário registrado com sucesso</response>
        /// <response code="400">Dados inválidos ou usuário já existe</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
