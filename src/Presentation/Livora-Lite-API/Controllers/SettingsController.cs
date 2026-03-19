using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Configurações - Gerencia parâmetros e configurações do sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly IAppSettingsRepository _appSettingsRepository;

        public SettingsController(IAppSettingsRepository appSettingsRepository)
        {
            _appSettingsRepository = appSettingsRepository;
        }

        /// <summary>
        /// Obtém uma configuração específica pela chave
        /// </summary>
        /// <param name="key">Chave da configuração</param>
        /// <returns>Dados da configuração</returns>
        [HttpGet("{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSetting(string key)
        {
            var setting = await _appSettingsRepository.GetByKeyAsync(key);
            if (setting == null)
                return NotFound();

            return Ok(setting);
        }

        /// <summary>
        /// Atualiza o tempo limite de sessão do sistema
        /// </summary>
        /// <param name="sessionTimeoutMinutes">Tempo em minutos (1-1440)</param>
        /// <returns>Confirmação da atualização</returns>
        [HttpPost("update-timeout")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateTimeout([FromBody] int sessionTimeoutMinutes)
        {
            if (sessionTimeoutMinutes < 1 || sessionTimeoutMinutes > 1440)
            {
                return BadRequest(new { message = "Timeout deve ser entre 1 e 1440 minutos." });
            }

            var setting = new AppSettings
            {
                Key = "SessionTimeoutMinutes",
                Value = sessionTimeoutMinutes.ToString()
            };

            await _appSettingsRepository.CreateOrUpdateAsync(setting);

            return Ok(new { message = "Timeout de sessão atualizado com sucesso." });
        }

        /// <summary>
        /// Obtém todas as configurações do sistema
        /// </summary>
        /// <returns>Lista de todas as configurações</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIndex()
        {
            var timeoutSetting = await _appSettingsRepository.GetByKeyAsync("SessionTimeoutMinutes");
            var timeout = timeoutSetting?.Value ?? "5";
            
            return Ok(new { SessionTimeout = timeout });
        }
    }
}
