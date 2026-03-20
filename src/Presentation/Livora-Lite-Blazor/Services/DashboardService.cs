using Livora_Lite.Application.DTO;
using System.Net.Http.Json;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Implementação do serviço de dashboard
    /// Gerencia a comunicação com a API para obter dados do dashboard
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DashboardService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
        }

        /// <summary>
        /// Obtém o dashboard para Admin
        /// </summary>
        public async Task<AdminDashboardDTO> GetAdminDashboardAsync()
        {
            try
            {
                Console.WriteLine($"[DashboardService] === REQUISIÇÃO: Admin Dashboard ===");
                Console.WriteLine($"[DashboardService] Endpoint: api/dashboard/admin");
                Console.WriteLine($"[DashboardService] Base Address: {_httpClient.BaseAddress}");
                
                var response = await _httpClient.GetAsync("api/dashboard/admin");

                Console.WriteLine($"[DashboardService] Status Code: {response.StatusCode}");
                Console.WriteLine($"[DashboardService] IsSuccessStatusCode: {response.IsSuccessStatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AdminDashboardDTO>();
                    Console.WriteLine($"[DashboardService] ✓ Dados recebidos com sucesso");
                    return result ?? new AdminDashboardDTO();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DashboardService] ✗ Erro: {response.StatusCode}");
                    Console.WriteLine($"[DashboardService] Resposta: {content}");
                    return new AdminDashboardDTO();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] ✗ Exceção: {ex.GetType().Name}");
                Console.WriteLine($"[DashboardService] Mensagem: {ex.Message}");
                return new AdminDashboardDTO();
            }
        }

        /// <summary>
        /// Obtém o dashboard para Owner (Proprietário)
        /// </summary>
        public async Task<OwnerDashboardDTO> GetOwnerDashboardAsync(int userId)
        {
            try
            {
                Console.WriteLine($"[DashboardService] === REQUISIÇÃO: Owner Dashboard ===");
                Console.WriteLine($"[DashboardService] Endpoint: api/dashboard/owner/{userId}");
                
                var response = await _httpClient.GetAsync($"api/dashboard/owner/{userId}");

                Console.WriteLine($"[DashboardService] Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OwnerDashboardDTO>();
                    Console.WriteLine($"[DashboardService] ✓ Dados recebidos com sucesso");
                    return result ?? new OwnerDashboardDTO();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DashboardService] ✗ Erro: {response.StatusCode}");
                    Console.WriteLine($"[DashboardService] Resposta: {content}");
                    return new OwnerDashboardDTO();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] ✗ Exceção: {ex.GetType().Name} - {ex.Message}");
                return new OwnerDashboardDTO();
            }
        }

        /// <summary>
        /// Obtém o dashboard para Tenant (Inquilino)
        /// </summary>
        public async Task<TenantDashboardDTO> GetTenantDashboardAsync(int userId)
        {
            try
            {
                Console.WriteLine($"[DashboardService] === REQUISIÇÃO: Tenant Dashboard ===");
                Console.WriteLine($"[DashboardService] Endpoint: api/dashboard/tenant/{userId}");
                
                var response = await _httpClient.GetAsync($"api/dashboard/tenant/{userId}");

                Console.WriteLine($"[DashboardService] Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TenantDashboardDTO>();
                    Console.WriteLine($"[DashboardService] ✓ Dados recebidos com sucesso");
                    return result ?? new TenantDashboardDTO();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DashboardService] ✗ Erro: {response.StatusCode}");
                    Console.WriteLine($"[DashboardService] Resposta: {content}");
                    return new TenantDashboardDTO();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] ✗ Exceção: {ex.GetType().Name} - {ex.Message}");
                return new TenantDashboardDTO();
            }
        }
    }
}
