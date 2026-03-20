using System.Linq;

namespace Livora_Lite_API.Middleware
{
    /// <summary>
    /// Middleware para registrar informações de autenticação em requisições
    /// </summary>
    public class AuthenticationLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Apenas registrar requisições para endpoints protegidos
            var path = context.Request.Path.Value ?? "";
            
            if (path.StartsWith("/api/dashboard", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/users", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/property", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"\n[AuthMiddleware] === REQUISIÇÃO RECEBIDA ===");
                Console.WriteLine($"[AuthMiddleware] Path: {context.Request.Method} {path}");

                // Verificar se hay header Authorization
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    Console.WriteLine($"[AuthMiddleware] Authorization Header: Presente");
                    if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = authHeader.Substring("Bearer ".Length);
                        Console.WriteLine($"[AuthMiddleware] Token Length: {token.Length} chars");
                        Console.WriteLine($"[AuthMiddleware] Token (primeiros 50 chars): {token.Substring(0, Math.Min(50, token.Length))}...");
                    }
                    else
                    {
                        Console.WriteLine($"[AuthMiddleware] ✗ Header não começa com 'Bearer'");
                    }
                }
                else
                {
                    Console.WriteLine($"[AuthMiddleware] ✗ NENHUM header Authorization encontrado!");
                }

                // Mostrar todos os headers relevantes
                Console.WriteLine($"[AuthMiddleware] Headers totais: {context.Request.Headers.Count}");
                foreach (var header in context.Request.Headers.Where(h => 
                    h.Key.Contains("Auth", StringComparison.OrdinalIgnoreCase) ||
                    h.Key.Contains("Bearer", StringComparison.OrdinalIgnoreCase) ||
                    h.Key == "Origin" ||
                    h.Key == "Referer"))
                {
                    Console.WriteLine($"[AuthMiddleware]   - {header.Key}: {header.Value.FirstOrDefault()?.Substring(0, Math.Min(50, header.Value.FirstOrDefault()?.Length ?? 0))}");
                }
            }

            await _next(context);
        }
    }
}
