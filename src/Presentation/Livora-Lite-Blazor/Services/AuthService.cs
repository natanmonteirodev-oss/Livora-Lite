using Livora_Lite.Application.DTO;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Implementação do serviço de autenticação
    /// Gerencia a comunicação com a API para autenticação de usuários
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string? _token;
        private UserDTO? _currentUser;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
            Console.WriteLine($"[AuthService] Serviço inicializado com base URL: {_httpClient.BaseAddress}");
        }

        /// <summary>
        /// Realiza o login de um usuário na API
        /// </summary>
        public async Task<AuthResponseDTO> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequestDTO
                {
                    Email = email,
                    Password = password
                };

                Console.WriteLine($"[AuthService] === INICIANDO LOGIN ===");
                Console.WriteLine($"[AuthService] Email: {email}");
                Console.WriteLine($"[AuthService] Base Address: {_httpClient.BaseAddress}");
                Console.WriteLine($"[AuthService] Endpoint: api/auth/login");

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

                Console.WriteLine($"[AuthService] Status Code: {response.StatusCode}");
                Console.WriteLine($"[AuthService] IsSuccessStatusCode: {response.IsSuccessStatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

                    Console.WriteLine($"[AuthService] Resposta recebida:");
                    Console.WriteLine($"[AuthService]   - Success: {result?.Success}");
                    Console.WriteLine($"[AuthService]   - Message: {result?.Message}");
                    Console.WriteLine($"[AuthService]   - User: {(result?.User != null ? $"{result.User.FirstName} {result.User.LastName}" : "null")}");
                    Console.WriteLine($"[AuthService]   - Token: {(result?.Token != null ? "Recebido" : "null")}");

                    if (result != null && result.Success && result.Token != null && result.User != null)
                    {
                        _token = result.Token;
                        _currentUser = result.User;
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _token);
                        Console.WriteLine($"[AuthService] ✓ Login bem-sucedido!");
                        Console.WriteLine($"[AuthService] ✓ Token armazenado (será enviado em proximas requisições)");
                        Console.WriteLine($"[AuthService] ✓ Header Authorization configurado");
                    }

                    return result ?? new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Resposta inválida do servidor",
                        Token = null,
                        User = null
                    };
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[AuthService] Erro HTTP {response.StatusCode}");
                    Console.WriteLine($"[AuthService] Resposta: {content}");

                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = $"Erro {(int)response.StatusCode}: Credenciais inválidas ou servidor indisponível",
                        Token = null,
                        User = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[AuthService] HttpRequestException: {ex.Message}");
                Console.WriteLine($"[AuthService] InnerException: {ex.InnerException?.Message}");
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Erro de conexão: {ex.Message}. Verifique se a API está rodando em {_httpClient.BaseAddress}",
                    Token = null,
                    User = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] Erro geral: {ex.GetType().Name} - {ex.Message}");
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Erro ao realizar login: {ex.Message}",
                    Token = null,
                    User = null
                };
            }
        }

        /// <summary>
        /// Registra um novo usuário na API
        /// </summary>
        public async Task<AuthResponseDTO> RegisterAsync(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            try
            {
                var registerRequest = new RegisterRequestDTO
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

                    if (result != null && result.Success && result.Token != null && result.User != null)
                    {
                        _token = result.Token;
                        _currentUser = result.User;
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _token);
                    }

                    return result ?? new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Resposta inválida do servidor",
                        Token = null,
                        User = null
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                    return errorContent ?? new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Erro ao registrar usuário",
                        Token = null,
                        User = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Erro ao registrar usuário: {ex.Message}",
                    Token = null,
                    User = null
                };
            }
        }

        /// <summary>
        /// Realiza o logout do usuário
        /// </summary>
        public async Task LogoutAsync()
        {
            _token = null;
            _currentUser = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Verifica se o usuário está autenticado
        /// </summary>
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_token) && _currentUser != null;
        }

        /// <summary>
        /// Obtém o token JWT armazenado
        /// </summary>
        public string? GetToken()
        {
            return _token;
        }

        /// <summary>
        /// Obtém o usuário autenticado
        /// </summary>
        public UserDTO? GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Atualiza o usuário atual
        /// </summary>
        public void SetCurrentUser(UserDTO? user)
        {
            _currentUser = user;
        }

        /// <summary>
        /// Atualiza o token armazenado
        /// </summary>
        public void SetToken(string? token)
        {
            _token = token;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
