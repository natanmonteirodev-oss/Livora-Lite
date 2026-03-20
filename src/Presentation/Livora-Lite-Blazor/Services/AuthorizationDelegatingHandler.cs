using System.Net.Http.Headers;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Delegating handler que injeta o token JWT automaticamente em todas as requisições
    /// </summary>
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;

        public AuthorizationDelegatingHandler(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Intercepta cada requisição e adiciona o token JWT se houver
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Obter o token do serviço de autenticação
                var token = _authService.GetToken();

                Console.WriteLine($"[AuthHandler] === INTERCEPTANDO REQUISIÇÃO ===");
                Console.WriteLine($"[AuthHandler] URL: {request.RequestUri}");
                Console.WriteLine($"[AuthHandler] Método: {request.Method}");
                Console.WriteLine($"[AuthHandler] Token disponível: {(!string.IsNullOrEmpty(token) ? "SIM" : "NÃO")}");

                if (!string.IsNullOrEmpty(token))
                {
                    // Adicionar o token ao header Authorization
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    Console.WriteLine($"[AuthHandler] ✓ Token adicionado ao header Authorization");
                    Console.WriteLine($"[AuthHandler] Tamanho do token: {token.Length} caracteres");
                    Console.WriteLine($"[AuthHandler] Primeiros 20 chars: {token.Substring(0, Math.Min(20, token.Length))}...");
                }
                else
                {
                    Console.WriteLine($"[AuthHandler] ✗ Nenhum token disponível - requisição será enviada sem autenticação");
                }

                // Continuar com a requisição
                Console.WriteLine($"[AuthHandler] → Enviando requisição para API...");
                var response = await base.SendAsync(request, cancellationToken);

                Console.WriteLine($"[AuthHandler] ← Resposta recebida");
                Console.WriteLine($"[AuthHandler] Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    Console.WriteLine($"[AuthHandler] Resposta de erro: {content}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthHandler] ✗ Erro ao processar requisição: {ex.GetType().Name}");
                Console.WriteLine($"[AuthHandler] Mensagem: {ex.Message}");
                Console.WriteLine($"[AuthHandler] StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
