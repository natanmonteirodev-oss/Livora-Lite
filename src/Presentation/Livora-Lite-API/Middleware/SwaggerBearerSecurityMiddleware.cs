using System.Text;
using System.Text.Json;

namespace Livora_Lite_API.Middleware
{
    /// <summary>
    /// Middleware para injetar suporte a Bearer Token (JWT) no documento OpenAPI/Swagger
    /// </summary>
    public class SwaggerBearerSecurityMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerBearerSecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Apenas interceptar requisições para o documento Swagger JSON
            if (context.Request.Path.StartsWithSegments("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[SwaggerBearerSecurityMiddleware] ✓ Interceptando requisição Swagger!");
                try
                {
                    // Capturar a resposta original
                    var originalBodyStream = context.Response.Body;
                    using (var memoryStream = new MemoryStream())
                    {
                        context.Response.Body = memoryStream;

                        // Chamar o próximo middleware
                        await _next(context);

                        // Ler a resposta
                        memoryStream.Position = 0;
                        var content = await new StreamReader(memoryStream).ReadToEndAsync();
                        Console.WriteLine($"[SwaggerBearerSecurityMiddleware] Conteúdo original tem {content.Length} bytes");
                        
                        // Modificar o conteúdo JSON inserindo o esquema de segurança
                        var modifiedContent = InjectBearerSecurityManually(content);
                        Console.WriteLine($"[SwaggerBearerSecurityMiddleware] Conteúdo modificado tem {modifiedContent.Length} bytes");

                        // Escrever a resposta modificada
                        context.Response.Body = originalBodyStream;
                        var modifiedBytes = Encoding.UTF8.GetBytes(modifiedContent);
                        context.Response.ContentLength = modifiedBytes.Length;
                        await originalBodyStream.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
                        Console.WriteLine("[SwaggerBearerSecurityMiddleware] ✓ Resposta modificada escrita!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SwaggerBearerSecurityMiddleware] ✗ Erro: {ex.Message}");
                    Console.WriteLine($"[SwaggerBearerSecurityMiddleware] StackTrace: {ex.StackTrace}");
                    // Se algo falhar, apenas passar adiante sem modificação
                    await _next(context);
                }
            }
            else
            {
                // Para outras requisições, apenas passar adiante
                await _next(context);
            }
        }

        private static string InjectBearerSecurityManually(string jsonContent)
        {
            try
            {
                // Se já tem securitySchemes, não fazer nada
                if (jsonContent.Contains("\"securitySchemes\""))
                {
                    Console.WriteLine("[SwaggerBearerSecurityMiddleware] securitySchemes já existe, não injetando");
                    return jsonContent;
                }

                Console.WriteLine("[SwaggerBearerSecurityMiddleware] Injetando securitySchemes...");

                // Procurar por "schemas" com espaçamento flexível
                var schemasPattern = "\"schemas\"";
                var schemasIndex = jsonContent.IndexOf(schemasPattern);
                
                if (schemasIndex > 0)
                {
                    Console.WriteLine("[SwaggerBearerSecurityMiddleware] Encontrado 'schemas' em posição " + schemasIndex);

                    // Encontrar o próximo { após "schemas"
                    var openBraceIndex = jsonContent.IndexOf("{", schemasIndex);
                    if (openBraceIndex > 0)
                    {
                        // Encontrar o próximo } que fecha schemas (contando braces)
                        var openBraces = 1; // Começamos após o primeiro {
                        var currentIndex = openBraceIndex + 1;
                        
                        while (currentIndex < jsonContent.Length && openBraces > 0)
                        {
                            var ch = jsonContent[currentIndex];
                            if (ch == '{') openBraces++;
                            else if (ch == '}') openBraces--;
                            
                            if (openBraces == 0)
                            {
                                // Encontramos o fechador de schemas
                                var insertPos = currentIndex + 1;
                                Console.WriteLine("[SwaggerBearerSecurityMiddleware] Schemas fecha em posição " + insertPos);

                                // Injetar securitySchemes APÓS schemas
                                var injection = ",\"securitySchemes\":{\"Bearer\":{\"type\":\"http\",\"scheme\":\"bearer\",\"bearerFormat\":\"JWT\",\"description\":\"JWT token. Obtenha um token válido fazendo POST em /api/Auth/login\"}}";
                                var result = jsonContent.Insert(insertPos, injection);
                                Console.WriteLine("[SwaggerBearerSecurityMiddleware] Injeção realizada! Novo tamanho: " + result.Length);
                                
                                // SEGUNDO PASSO: Adicionar requisição de segurança aos endpoints protegidos
                                result = AddSecurityRequirementToOperations(result);
                                
                                return result;
                            }
                            currentIndex++;
                        }
                    }
                    
                    Console.WriteLine("[SwaggerBearerSecurityMiddleware] Não conseguiu encontrar { após schemas ou fechador");
                }
                else
                {
                    Console.WriteLine("[SwaggerBearerSecurityMiddleware] 'schemas' não encontrado em nenhuma forma");
                }

                return jsonContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SwaggerBearerSecurityMiddleware] Erro ao injetar: {ex.Message}");
                return jsonContent;
            }
        }

        // Método para adicionar requisição de segurança aos endpoints protegidos
        private static string AddSecurityRequirementToOperations(string jsonContent)
        {
            try
            {
                Console.WriteLine("[SwaggerBearerSecurityMiddleware] ========== ADICIONANDO SECURITY AOS ENDPOINTS ==========");
                
                // Endpoints públicos que NÃO requerem segurança
                var publicPaths = new[] { "/auth/login", "/auth/register" };

                // Procurar por cada operação HTTP e adicionar security DEPOIS de responses
                string[] httpMethods = { "\"get\"", "\"post\"", "\"put\"", "\"delete\"", "\"patch\"" };
                var result = jsonContent;
                int modificacoes = 0;

                foreach (var method in httpMethods)
                {
                    Console.WriteLine($"[SwaggerBearerSecurityMiddleware] Procurando {method} operations...");
                    
                    // Procurar por padrão: método seguido de { e depois responses:{...}
                    // Exemplo: "get":{"tags":...,"responses":{...}}
                    // Vamos procurar por "responses":{ depois de cada método e adicionar security ANTES do } que fecha responses
                    
                    int searchPos = 0;
                    int operacoesTratadas = 0;
                    
                    while ((searchPos = result.IndexOf(method + "\":{", searchPos)) >= 0)
                    {
                        // Encontrou um método. Agora procura "responses":{ a partir daqui
                        int researchStart = searchPos + method.Length;
                        int responsesPos = result.IndexOf("\"responses\":{", researchStart);
                        
                        if (responsesPos > 0 && responsesPos < searchPos + 2000) // responses deve estar próximo
                        {
                            // Verificar se é endpoint público
                            int pathStart = result.LastIndexOf("\"", searchPos - 10);
                            int pathEnd = result.IndexOf("\"", pathStart + 1);
                            string pathName = pathEnd > pathStart ? result.Substring(pathStart + 1, pathEnd - pathStart - 1) : "";
                            
                            bool isPublic = pathName.Contains("/login", StringComparison.OrdinalIgnoreCase) || 
                                          pathName.Contains("/register", StringComparison.OrdinalIgnoreCase);
                            
                            if (!isPublic)
                            {
                                // Encontrar o } que fecha "responses"
                                int braceCount = 1;
                                int closePos = responsesPos + 13; // "responses":{
                                
                                while (closePos < result.Length && braceCount > 0)
                                {
                                    if (result[closePos] == '{') braceCount++;
                                    else if (result[closePos] == '}') braceCount--;
                                    closePos++;
                                }
                                
                                // closePos aponta APÓS o }
                                // Verificar se já não tem "security"
                                if (closePos < result.Length  )
                                {
                                    string after = result.Substring(closePos, Math.Min(50, result.Length - closePos));
                                    
                                    if (!after.Contains("\"security\""))
                                    {
                                        // Inserir "security" ANTES do }
                                        string injection = ",\"security\":[{\"Bearer\":[]}]";
                                        result = result.Insert(closePos - 1, injection);
                                        Console.WriteLine($"[SwaggerBearerSecurityMiddleware] ✓ Security adicionado ao {method.Replace("\"", "")} em '{pathName}'");
                                        modificacoes++;
                                        
                                        // Ajustar posição de busca
                                        searchPos = closePos + injection.Length;
                                    }
                                    else
                                    {
                                        searchPos = closePos;
                                    }
                                }
                                else
                                {
                                    searchPos += method.Length;
                                }
                            }
                            else
                            {
                                searchPos = responsesPos;
                            }
                        }
                        else
                        {
                            searchPos += method.Length;
                        }
                        
                        operacoesTratadas++;
                        if (operacoesTratadas > 200) break; // Limite de segurança
                    }
                }

                Console.WriteLine($"[SwaggerBearerSecurityMiddleware] ✓ Adicionado security em {modificacoes} operações");
                Console.WriteLine($"[SwaggerBearerSecurityMiddleware] Tamanho antes: {jsonContent.Length}, depois: {result.Length}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SwaggerBearerSecurityMiddleware] ✗ ERRO: {ex.Message}");
                return jsonContent;
            }
        }
    }
}
