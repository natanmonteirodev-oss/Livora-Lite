using Livora_Lite.Application.DTO;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Serviço Blazor para operações de propriedades via API
    /// Encapsula as chamadas HTTP para o endpoint /api/properties
    /// </summary>
    public class BlazorPropertyService
    {
        private readonly ApiHttpClientService _apiClient;
        private readonly ILogger<BlazorPropertyService> _logger;

        public BlazorPropertyService(ApiHttpClientService apiClient, ILogger<BlazorPropertyService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todas as propriedades
        /// </summary>
        public async Task<List<PropertyDTO>?> GetAllPropertiesAsync()
        {
            try
            {
                return await _apiClient.GetAsync<List<PropertyDTO>>("api/properties");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as propriedades");
                throw;
            }
        }

        /// <summary>
        /// Obtém uma propriedade específica por ID
        /// </summary>
        public async Task<PropertyDTO?> GetPropertyByIdAsync(int id)
        {
            try
            {
                return await _apiClient.GetAsync<PropertyDTO>($"api/properties/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter propriedade {id}");
                throw;
            }
        }

        /// <summary>
        /// Cria uma nova propriedade
        /// </summary>
        public async Task<dynamic?> CreatePropertyAsync(CreatePropertyRequestDTO request)
        {
            try
            {
                return await _apiClient.PostAsync<dynamic>("api/properties", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar propriedade");
                throw;
            }
        }

        /// <summary>
        /// Atualiza uma propriedade existente
        /// </summary>
        public async Task<dynamic?> UpdatePropertyAsync(UpdatePropertyRequestDTO request)
        {
            try
            {
                return await _apiClient.PutAsync<dynamic>($"api/properties/{request.Id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar propriedade {request.Id}");
                throw;
            }
        }

        /// <summary>
        /// Deleta uma propriedade
        /// </summary>
        public async Task<bool> DeletePropertyAsync(int id)
        {
            try
            {
                return await _apiClient.DeleteAsync($"api/properties/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar propriedade {id}");
                throw;
            }
        }
    }
}
